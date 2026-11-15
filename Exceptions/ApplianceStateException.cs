using System;

namespace HomeApplianceRepair.Exceptions
{
    // استثناء يُرمى عندما تكون العملية غير مسموحة في حالة الصيانة الحالية،
    // مثل تسليم جهاز لم يُصلَّح بعد.
    public class ApplianceStateException : ApplicationException
    {
        public ApplianceStateException() :
            base("This operation is not allowed in the current appliance state.")
        { }

        public ApplianceStateException(string message) : base(message)
        { }
    }// نهاية الفئة ApplianceStateException
}// نهاية فضاء الأسماء Exceptions
