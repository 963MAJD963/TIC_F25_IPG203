using System;
using HomeApplianceRepair.Exceptions;
using HomeApplianceRepair.Utilities;

namespace HomeApplianceRepair.Models
{
    // فئة مشتقة: المكيف هو جهاز منزلي.
    public class AirConditioner : Appliance
    {
        private const double GasRefillFee = 60.0;      // أجرة تعبئة الغاز
        private const double HighCapacityFee = 40.0;   // رسم الاستطاعة العالية
        private const int HighCapacityBtu = 24000;     // حد الاستطاعة العالية

        private int capacityBtu;

        public AirConditioner(string serialNumber, string brand, string ownerName,
                              DateTime receivedDate, DateTime dueDate,
                              int capacityBtu, bool needsGasRefill)
            : base(serialNumber, brand, ownerName, receivedDate, dueDate)
        {
            CapacityBtu = capacityBtu;
            NeedsGasRefill = needsGasRefill;
        }// نهاية الباني

        // استطاعة المكيف بوحدة BTU (لا تُعدَّل من خارج الفئة)
        public int CapacityBtu
        {
            get => capacityBtu;
            private set
            {
                if (!Validator.IsPositive(value))
                    throw new InvalidApplianceDataException("Capacity must be greater than zero.");
                capacityBtu = value;
            }
        }// نهاية CapacityBtu

        // هل يحتاج المكيف إلى تعبئة غاز؟
        public bool NeedsGasRefill { get; private set; }

        public override string ApplianceType => "AirConditioner";

        // تكلفة المكيف = التكلفة الأساسية + تعبئة الغاز (عند الحاجة) + رسم الاستطاعة العالية
        public override double CalculateRepairCost()
        {
            double cost = BaseCost;
            if (NeedsGasRefill) cost += GasRefillFee;
            if (capacityBtu >= HighCapacityBtu) cost += HighCapacityFee;
            return cost;
        }// نهاية CalculateRepairCost

        public override string GetSummary()
        {
            return base.GetSummary() + string.Format(" | {0} BTU, gas refill: {1}",
                capacityBtu, NeedsGasRefill ? "yes" : "no");
        }// نهاية GetSummary
    }// نهاية الفئة AirConditioner
}// نهاية فضاء الأسماء Models
