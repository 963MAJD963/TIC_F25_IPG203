using System;
using HomeApplianceRepair.Events;
using HomeApplianceRepair.Exceptions;
using HomeApplianceRepair.Interfaces;
using HomeApplianceRepair.Utilities;

namespace HomeApplianceRepair.Models
{
    // الفئة المجردة: تنفذ الواجهة IRepairable وتجمع الخصائص المشتركة لكل الأجهزة،
    // وتترك حساب تكلفة الإصلاح للفئات المشتقة.
    public abstract class Appliance : IRepairable
    {
        // أجرة ساعة العمل الواحدة (مشتركة بين كل الأنواع، وخاصة بهذه الفئة)
        private const double LaborRate = 20.0;

        // ---------- أعضاء ثابتة (مشتركة بين كل الكائنات) ----------
        private static int totalReceived = 0;
        private static int inRepairCount = 0;
        private static int repairedCount = 0;
        private static double totalCollected = 0;

        // عدد الأجهزة المستلمة
        public static int TotalReceived => totalReceived;
        // عدد الأجهزة قيد الصيانة حاليا
        public static int InRepairCount => inRepairCount;
        // عدد الأجهزة التي انتهى إصلاحها (حالة Repaired أو Delivered)
        public static int RepairedCount => repairedCount;
        // مجموع المبالغ المقبوضة من الزبائن عند التسليم
        public static double TotalCollected => totalCollected;

        // يسجل استلام الجهاز في المركز. يستدعيها RepairCenter بعد قبول الجهاز،
        // حتى لا يُحسب جهاز فشل إنشاؤه (مثل سعة غير صالحة في الفئة المشتقة).
        internal static void RegisterReceived()
        {
            totalReceived++;
        }// نهاية RegisterReceived

        // ---------- الحقول الخاصة ----------
        private readonly string serialNumber;   // لا يتغير بعد الإنشاء
        private readonly DateTime receivedDate; // لا يتغير بعد الإنشاء
        private string brand;
        private string ownerName;
        private DateTime dueDate;
        private string status;
        private double laborHours;
        private double partsCost;

        // ---------- الأحداث ----------
        // يُطلق عند كل تغير في حالة الصيانة
        public event ApplianceNotify StatusChanged;
        // يُطلق عند تجاوز موعد التسليم والجهاز لم يُسلَّم بعد
        public event ApplianceNotify DelayDetected;

        // ---------- الباني ----------
        protected Appliance(string serialNumber, string brand, string ownerName,
                            DateTime receivedDate, DateTime dueDate)
        {
            if (!Validator.IsValidSerial(serialNumber))
                throw new InvalidApplianceDataException(
                    "Serial number must follow the format LL-YYYY-NNNN, e.g. FR-2026-0001.");
            if (!Validator.IsValidDate(receivedDate))
                throw new InvalidApplianceDataException("Received date is not valid.");

            this.serialNumber = serialNumber;
            this.receivedDate = receivedDate;

            // الخصائص التالية تتحقق من صحة القيم قبل حفظها
            Brand = brand;
            OwnerName = ownerName;
            DueDate = dueDate;

            status = ApplianceStatus.Received;
        }// نهاية الباني

        // ---------- الخصائص ----------
        // خصائص للقراءة فقط
        public string SerialNumber => serialNumber;
        public DateTime ReceivedDate => receivedDate;
        public string Status => status;

        public string Brand
        {
            get => brand;
            set
            {
                if (!Validator.IsValidText(value))
                    throw new InvalidApplianceDataException("Brand can not be empty.");
                CheckNotDelivered();
                brand = value.Trim();
            }
        }// نهاية Brand

        public string OwnerName
        {
            get => ownerName;
            set
            {
                if (!Validator.IsValidText(value))
                    throw new InvalidApplianceDataException("Owner name can not be empty.");
                CheckNotDelivered();
                ownerName = value.Trim();
            }
        }// نهاية OwnerName

        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                if (!Validator.IsValidDueDate(receivedDate, value))
                    throw new InvalidApplianceDataException(
                        "Due date must be a valid date that is not before the received date.");
                CheckNotDelivered();
                dueDate = value;
            }
        }// نهاية DueDate

        public double LaborHours
        {
            get => laborHours;
            set
            {
                if (!Validator.IsNotNegative(value))
                    throw new InvalidApplianceDataException("Labor hours can not be negative.");
                CheckNotDelivered();
                laborHours = value;
            }
        }// نهاية LaborHours

        public double PartsCost
        {
            get => partsCost;
            set
            {
                if (!Validator.IsNotNegative(value))
                    throw new InvalidApplianceDataException("Parts cost can not be negative.");
                CheckNotDelivered();
                partsCost = value;
            }
        }// نهاية PartsCost

        // اسم نوع الجهاز، تحدده كل فئة مشتقة
        public abstract string ApplianceType { get; }

        // التكلفة الأساسية المشتركة: أجرة العمل + ثمن القطع.
        // تستفيد منها الفئات المشتقة بدل تكرار المعادلة في كل فئة.
        protected double BaseCost => laborHours * LaborRate + partsCost;

        // ---------- الدالة المجردة ----------
        // لكل نوع جهاز طريقته الخاصة في حساب تكلفة الإصلاح
        public abstract double CalculateRepairCost();

        // ---------- عمليات الصيانة (تنفيذ الواجهة) ----------
        public void StartRepair()
        {
            if (status != ApplianceStatus.Received)
                throw new ApplianceStateException(
                    "Repair of " + serialNumber + " can start only from the Received state.");

            ChangeStatus(ApplianceStatus.InRepair, "repair started");
        }// نهاية StartRepair

        public void CompleteRepair()
        {
            if (status != ApplianceStatus.InRepair)
                throw new ApplianceStateException(
                    "Repair of " + serialNumber + " can be completed only while it is in repair.");

            ChangeStatus(ApplianceStatus.Repaired,
                string.Format("repair completed, cost = {0:F2} $", CalculateRepairCost()));
        }// نهاية CompleteRepair

        public void DeliverToCustomer()
        {
            if (status != ApplianceStatus.Repaired)
                throw new ApplianceStateException(
                    serialNumber + " can be delivered only after the repair is completed.");

            double cost = CalculateRepairCost();
            totalCollected += cost;
            ChangeStatus(ApplianceStatus.Delivered,
                string.Format("delivered to {0}, paid {1:F2} $", ownerName, cost));
        }// نهاية DeliverToCustomer

        // يطلق الحدث DelayDetected إذا تجاوز اليوم موعد التسليم والجهاز لم يُسلَّم
        public bool CheckDelay(DateTime today)
        {
            if (status == ApplianceStatus.Delivered || today.Date <= dueDate.Date)
                return false;

            int lateDays = (today.Date - dueDate.Date).Days;
            OnDelayDetected(string.Format("delivery is late by {0} day(s)", lateDays));
            return true;
        }// نهاية CheckDelay

        // ---------- الوصف ----------
        public virtual string GetSummary()
        {
            return string.Format("[{0}] {1} {2} | owner: {3} | status: {4} | due: {5:yyyy-MM-dd} | cost: {6:F2} $",
                serialNumber, ApplianceType, brand, ownerName, status, dueDate, CalculateRepairCost());
        }// نهاية GetSummary

        public override string ToString()
        {
            return GetSummary();
        }// نهاية ToString

        // ---------- إطلاق الأحداث ----------
        protected virtual void OnStatusChanged(string message)
        {
            // نستدعي التفويض فقط إذا وُجد مشترك واحد على الأقل
            StatusChanged?.Invoke(this, message);
        }// نهاية OnStatusChanged

        protected virtual void OnDelayDetected(string message)
        {
            DelayDetected?.Invoke(this, message);
        }// نهاية OnDelayDetected

        // ---------- دوال مساعدة خاصة ----------
        // تغير الحالة وتحدّث العدادات الثابتة ثم تطلق الحدث
        private void ChangeStatus(string newStatus, string message)
        {
            if (status == ApplianceStatus.InRepair) inRepairCount--;
            if (newStatus == ApplianceStatus.InRepair) inRepairCount++;
            if (newStatus == ApplianceStatus.Repaired) repairedCount++;

            status = newStatus;
            OnStatusChanged(message);
        }// نهاية ChangeStatus

        // بيانات الجهاز المسلَّم لا يمكن تعديلها بعد التسليم
        private void CheckNotDelivered()
        {
            if (status == ApplianceStatus.Delivered)
                throw new ApplianceStateException(
                    serialNumber + " is already delivered, its data can not be modified.");
        }// نهاية CheckNotDelivered
    }// نهاية الفئة Appliance
}// نهاية فضاء الأسماء Models
