using System;
using HomeApplianceRepair.Exceptions;
using HomeApplianceRepair.Utilities;

namespace HomeApplianceRepair.Models
{
    // فئة مشتقة: البراد هو جهاز منزلي (علاقة "هو نوع من").
    public class Refrigerator : Appliance
    {
        private const double CompressorFee = 80.0;     // أجرة إصلاح الضاغط
        private const double LargeCapacityFee = 15.0;  // رسم السعة الكبيرة
        private const int LargeCapacityLiters = 400;   // حد السعة الكبيرة

        private int capacityLiters;

        public Refrigerator(string serialNumber, string brand, string ownerName,
                            DateTime receivedDate, DateTime dueDate,
                            int capacityLiters, bool hasCompressorFault)
            : base(serialNumber, brand, ownerName, receivedDate, dueDate)
        {
            CapacityLiters = capacityLiters;
            HasCompressorFault = hasCompressorFault;
        }// نهاية الباني

        // سعة البراد باللتر (لا تُعدَّل من خارج الفئة)
        public int CapacityLiters
        {
            get => capacityLiters;
            private set
            {
                if (!Validator.IsPositive(value))
                    throw new InvalidApplianceDataException("Capacity must be greater than zero.");
                capacityLiters = value;
            }
        }// نهاية CapacityLiters

        // هل في الضاغط عطل؟
        public bool HasCompressorFault { get; private set; }

        public override string ApplianceType => "Refrigerator";

        // تكلفة البراد = التكلفة الأساسية + أجرة الضاغط (عند العطل) + رسم السعة الكبيرة
        public override double CalculateRepairCost()
        {
            double cost = BaseCost;
            if (HasCompressorFault) cost += CompressorFee;
            if (capacityLiters > LargeCapacityLiters) cost += LargeCapacityFee;
            return cost;
        }// نهاية CalculateRepairCost

        public override string GetSummary()
        {
            return base.GetSummary() + string.Format(" | {0} L, compressor fault: {1}",
                capacityLiters, HasCompressorFault ? "yes" : "no");
        }// نهاية GetSummary
    }// نهاية الفئة Refrigerator
}// نهاية فضاء الأسماء Models
