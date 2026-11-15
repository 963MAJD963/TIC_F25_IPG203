using System;
using HomeApplianceRepair.Exceptions;
using HomeApplianceRepair.Utilities;

namespace HomeApplianceRepair.Models
{
    // فئة مشتقة: الغسالة هي جهاز منزلي.
    public class WashingMachine : Appliance
    {
        private const double FrontLoaderFee = 25.0;  // صيانة الباب والأسطوانة (أمامية)
        private const double TopLoaderFee = 10.0;    // صيانة الغسالة العلوية
        private const double HeavyLoadFee = 20.0;    // رسم الحمولة الكبيرة
        private const double HeavyLoadKg = 9.0;      // حد الحمولة الكبيرة

        private double loadCapacityKg;

        public WashingMachine(string serialNumber, string brand, string ownerName,
                              DateTime receivedDate, DateTime dueDate,
                              double loadCapacityKg, bool isFrontLoader)
            : base(serialNumber, brand, ownerName, receivedDate, dueDate)
        {
            LoadCapacityKg = loadCapacityKg;
            IsFrontLoader = isFrontLoader;
        }// نهاية الباني

        // سعة الحمولة بالكيلوغرام (لا تُعدَّل من خارج الفئة)
        public double LoadCapacityKg
        {
            get => loadCapacityKg;
            private set
            {
                if (!Validator.IsPositive(value))
                    throw new InvalidApplianceDataException("Load capacity must be greater than zero.");
                loadCapacityKg = value;
            }
        }// نهاية LoadCapacityKg

        // هل الغسالة أمامية التحميل؟
        public bool IsFrontLoader { get; private set; }

        public override string ApplianceType => "WashingMachine";

        // تكلفة الغسالة = التكلفة الأساسية + أجرة الصيانة حسب النوع + رسم الحمولة الكبيرة
        public override double CalculateRepairCost()
        {
            double cost = BaseCost;
            cost += IsFrontLoader ? FrontLoaderFee : TopLoaderFee;
            if (loadCapacityKg >= HeavyLoadKg) cost += HeavyLoadFee;
            return cost;
        }// نهاية CalculateRepairCost

        public override string GetSummary()
        {
            return base.GetSummary() + string.Format(" | {0} kg, {1}",
                loadCapacityKg, IsFrontLoader ? "front loader" : "top loader");
        }// نهاية GetSummary
    }// نهاية الفئة WashingMachine
}// نهاية فضاء الأسماء Models
