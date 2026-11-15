namespace HomeApplianceRepair.Utilities
{
    // فئة ثابتة تجمع أسماء حالات الصيانة في مكان واحد.
    // الفئة الثابتة لا يمكن إنشاء كائن منها وتحوي أعضاء ثابتة فقط.
    public static class ApplianceStatus
    {
        public const string Received = "Received";   // تم الاستلام
        public const string InRepair = "InRepair";   // قيد الصيانة
        public const string Repaired = "Repaired";   // تم الإصلاح
        public const string Delivered = "Delivered"; // تم التسليم
    }// نهاية الفئة ApplianceStatus
}// نهاية فضاء الأسماء Utilities
