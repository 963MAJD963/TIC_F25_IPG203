using System;
using HomeApplianceRepair.Exceptions;
using HomeApplianceRepair.Models;
using HomeApplianceRepair.Services;
using HomeApplianceRepair.Utilities;

namespace HomeApplianceRepair
{
    class Program
    {
        static void Main()
        {
            RepairCenter center = new RepairCenter("Damascus Home Appliance Repair Center");

            // ----- إنشاء أجهزة من أنواع مختلفة -----
            Refrigerator fridge = new Refrigerator("FR-2026-0001", "Samsung", "Ahmad Salem",
                new DateTime(2026, 11, 1), new DateTime(2026, 11, 8), 450, true);
            WashingMachine washer = new WashingMachine("WM-2026-0002", "LG", "Reem Khaled",
                new DateTime(2026, 11, 2), new DateTime(2026, 11, 6), 9.5, true);
            AirConditioner conditioner = new AirConditioner("AC-2026-0003", "Gree", "Rami Haddad",
                new DateTime(2026, 11, 3), new DateTime(2026, 11, 20), 24000, true);
            WashingMachine oldWasher = new WashingMachine("WM-2026-0004", "Zanussi", "Lina Nader",
                new DateTime(2026, 11, 4), new DateTime(2026, 11, 14), 6, false);

            // تحديد ساعات العمل وتكلفة القطع
            fridge.LaborHours = 3;     fridge.PartsCost = 45;
            washer.LaborHours = 2;     washer.PartsCost = 30;
            conditioner.LaborHours = 4; conditioner.PartsCost = 70;
            oldWasher.LaborHours = 1.5; oldWasher.PartsCost = 15;

            center.AddAppliance(fridge);
            center.AddAppliance(washer);
            center.AddAppliance(conditioner);
            center.AddAppliance(oldWasher);

            // ----- البيانات الخاطئة والعمليات غير المسموحة تُرفض بالاستثناءات -----
            try
            {
                // رقم تسلسلي بصيغة خاطئة
                center.AddAppliance(new Refrigerator("BAD-SERIAL", "Bosch", "Test",
                    new DateTime(2026, 11, 1), new DateTime(2026, 11, 5), 300, false));
            }
            catch (InvalidApplianceDataException e)
            {
                Console.WriteLine("Rejected data: " + e.Message);
            }

            try
            {
                washer.DeliverToCustomer(); // ما زالت في حالة Received
            }
            catch (ApplianceStateException e)
            {
                Console.WriteLine("Rejected operation: " + e.Message);
            }

            try
            {
                fridge.LaborHours = -2; // ساعات عمل سالبة
            }
            catch (InvalidApplianceDataException e)
            {
                Console.WriteLine("Rejected data: " + e.Message);
            }

            // ----- عمليات الصيانة تطلق الحدث StatusChanged -----
            Console.WriteLine();
            fridge.StartRepair();
            washer.StartRepair();
            conditioner.StartRepair();
            oldWasher.StartRepair();

            washer.CompleteRepair();
            fridge.CompleteRepair();
            oldWasher.CompleteRepair();
            oldWasher.DeliverToCustomer();

            // ----- فحص التأخير يطلق الحدث DelayDetected -----
            Console.WriteLine();
            int late = center.CheckDelays(new DateTime(2026, 11, 14));
            Console.WriteLine("Late appliances: " + late);

            // ----- معالجة القائمة كاملة بتعدد الأشكال -----
            Console.WriteLine();
            center.DisplayAll();
            Console.WriteLine();
            center.DisplayByStatus(ApplianceStatus.Repaired);

            Console.WriteLine();
            Console.WriteLine("Total cost of all appliances: {0:F2} $", center.CalculateTotalCost());
            Console.WriteLine("Total cost of Repaired appliances: {0:F2} $",
                center.CalculateTotalCost(ApplianceStatus.Repaired));

            // ----- الخصائص الثابتة -----
            Console.WriteLine();
            Console.WriteLine("Total received appliances : " + Appliance.TotalReceived);
            Console.WriteLine("Currently in repair       : " + Appliance.InRepairCount);
            Console.WriteLine("Repairs finished          : " + Appliance.RepairedCount);
            Console.WriteLine("Money collected           : {0:F2} $", Appliance.TotalCollected);
            Console.WriteLine("Failed validation checks  : " + Validator.FailedChecks);

            Console.WriteLine();
            center.DisplayNotifications();
        }// نهاية Main
    }// نهاية الفئة Program
}// نهاية فضاء الأسماء HomeApplianceRepair
