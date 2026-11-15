using System;

namespace HomeApplianceRepair.Interfaces
{
    // الواجهة: تحدد العمليات المشتركة بين جميع الأجهزة المنزلية في المركز.
    // أعضاء الواجهة عامة تلقائيا وبلا تنفيذ، وتنفذها الفئة المجردة Appliance.
    public interface IRepairable
    {
        // الرقم التسلسلي للجهاز (للقراءة فقط)
        string SerialNumber { get; }

        // حالة الصيانة الحالية (للقراءة فقط)
        string Status { get; }

        // بدء الإصلاح
        void StartRepair();

        // إنهاء الإصلاح
        void CompleteRepair();

        // تسليم الجهاز للزبون
        void DeliverToCustomer();

        // حساب تكلفة الإصلاح (لكل جهاز طريقته)
        double CalculateRepairCost();

        // نص يلخص حالة الجهاز
        string GetSummary();
    }// نهاية الواجهة IRepairable
}// نهاية فضاء الأسماء Interfaces
