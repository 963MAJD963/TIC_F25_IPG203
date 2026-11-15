using System;

namespace HomeApplianceRepair.Exceptions
{
    // استثناء يُرمى عندما تكون القيمة المدخلة غير صالحة،
    // مثل رقم تسلسلي بصيغة خاطئة أو ساعات عمل سالبة.
    public class InvalidApplianceDataException : ApplicationException
    {
        public InvalidApplianceDataException() :
            base("The supplied appliance data is not valid.")
        { }

        public InvalidApplianceDataException(string message) : base(message)
        { }
    }// نهاية الفئة InvalidApplianceDataException
}// نهاية فضاء الأسماء Exceptions
