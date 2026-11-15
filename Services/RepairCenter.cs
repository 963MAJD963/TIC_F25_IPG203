using System;
using System.Collections.Generic;
using HomeApplianceRepair.Exceptions;
using HomeApplianceRepair.Models;
using HomeApplianceRepair.Utilities;

namespace HomeApplianceRepair.Services
{
    // الفئة الرئيسية: تحتفظ بقائمة من نمط الفئة الأم Appliance وتعالجها بتعدد الأشكال.
    // وهي أيضا المشترك الذي يستقبل الأحداث التي تطلقها الأجهزة.
    public class RepairCenter
    {
        private readonly List<Appliance> appliances = new List<Appliance>();
        private readonly List<string> notifications = new List<string>();
        private string name;

        public RepairCenter(string name)
        {
            if (!Validator.IsValidText(name))
                throw new InvalidApplianceDataException("Center name can not be empty.");
            this.name = name.Trim();
        }// نهاية الباني

        public string Name => name;
        public int Count => appliances.Count;

        // يضيف جهازا إلى القائمة ويشترك في أحداثه
        public void AddAppliance(Appliance appliance)
        {
            if (appliance == null)
                throw new InvalidApplianceDataException("Appliance can not be null.");
            if (FindBySerial(appliance.SerialNumber) != null)
                throw new InvalidApplianceDataException(
                    "An appliance with serial " + appliance.SerialNumber + " already exists.");

            appliance.StatusChanged += HandleNotification;   // الاشتراك في الحدثين
            appliance.DelayDetected += HandleNotification;
            appliances.Add(appliance);
            Appliance.RegisterReceived();   // زيادة عداد الأجهزة المستلمة
        }// نهاية AddAppliance

        // يعيد الجهاز صاحب الرقم التسلسلي المطلوب، أو null إذا لم يوجد
        public Appliance FindBySerial(string serial)
        {
            foreach (Appliance appliance in appliances)
                if (appliance.SerialNumber == serial) return appliance;
            return null;
        }// نهاية FindBySerial

        // تعدد الأشكال: تُستدعى CalculateRepairCost الخاصة بالنوع الحقيقي
        // (Refrigerator أو WashingMachine ...) عبر مرجع من النمط Appliance.
        public double CalculateTotalCost()
        {
            double total = 0;
            foreach (Appliance appliance in appliances)
                total += appliance.CalculateRepairCost();
            return total;
        }// نهاية CalculateTotalCost

        // نسخة محملة بشكل زائد: مجموع تكلفة الأجهزة التي لها حالة معينة
        public double CalculateTotalCost(string status)
        {
            double total = 0;
            foreach (Appliance appliance in appliances)
                if (appliance.Status == status) total += appliance.CalculateRepairCost();
            return total;
        }// نهاية CalculateTotalCost

        // عدد الأجهزة التي لها حالة معينة
        public int CountByStatus(string status)
        {
            int count = 0;
            foreach (Appliance appliance in appliances)
                if (appliance.Status == status) count++;
            return count;
        }// نهاية CountByStatus

        // يفحص كل الأجهزة، والمتأخر منها يطلق الحدث DelayDetected
        public int CheckDelays(DateTime today)
        {
            int lateCount = 0;
            foreach (Appliance appliance in appliances)
                if (appliance.CheckDelay(today)) lateCount++;
            return lateCount;
        }// نهاية CheckDelays

        // يعرض حالة كل الأجهزة (يُختار GetSummary المناسب وقت التشغيل)
        public void DisplayAll()
        {
            Console.WriteLine("=== {0}: {1} appliance(s) ===", name, appliances.Count);
            foreach (Appliance appliance in appliances)
                Console.WriteLine(appliance.GetSummary());
        }// نهاية DisplayAll

        // يعرض الأجهزة التي لها حالة معينة
        public void DisplayByStatus(string status)
        {
            Console.WriteLine("--- Appliances with status {0} ---", status);
            foreach (Appliance appliance in appliances)
                if (appliance.Status == status) Console.WriteLine(appliance.GetSummary());
        }// نهاية DisplayByStatus

        // يعرض سجل الإشعارات
        public void DisplayNotifications()
        {
            Console.WriteLine("=== Notification log ({0}) ===", notifications.Count);
            foreach (string notification in notifications)
                Console.WriteLine(notification);
        }// نهاية DisplayNotifications

        // دالة معالجة الحدث: توقيعها يطابق توقيع التفويض ApplianceNotify
        private void HandleNotification(Appliance appliance, string message)
        {
            string line = string.Format("[NOTIFY] {0} {1}: {2}",
                appliance.ApplianceType, appliance.SerialNumber, message);
            notifications.Add(line);
            Console.WriteLine(line);
        }// نهاية HandleNotification
    }// نهاية الفئة RepairCenter
}// نهاية فضاء الأسماء Services
