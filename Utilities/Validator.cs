using System;

namespace HomeApplianceRepair.Utilities
{
    // فئة ثابتة فيها دوال التحقق من صحة البيانات المدخلة.
    public static class Validator
    {
        // التواريخ المقبولة تقع ضمن هذا المجال من السنوات
        private const int MinYear = 2000;
        private const int MaxYear = 2100;

        // خاصية ثابتة: عدد عمليات التحقق التي فشلت منذ بدء البرنامج
        public static int FailedChecks { get; private set; }

        // صيغة الرقم التسلسلي: حرفان - سنة - أربعة أرقام، مثال FR-2026-0001
        public static bool IsValidSerial(string serial)
        {
            bool valid = serial != null && serial.Length == 12
                && char.IsLetter(serial[0]) && char.IsLetter(serial[1])
                && serial[2] == '-' && serial[7] == '-';

            for (int i = 3; valid && i < 12; i++)
            {
                if (i == 7) continue; // نتخطى الشرطة الثانية
                if (!char.IsDigit(serial[i])) valid = false;
            }

            return Register(valid);
        }// نهاية IsValidSerial

        // النص (الماركة، اسم المالك...) يجب أن يحوي حرفا ظاهرا واحدا على الأقل
        public static bool IsValidText(string text)
        {
            return Register(text != null && text.Trim().Length > 0);
        }// نهاية IsValidText

        // التاريخ صالح إذا كانت سنته ضمن المجال المقبول
        public static bool IsValidDate(DateTime date)
        {
            return Register(IsInRange(date));
        }// نهاية IsValidDate

        // تاريخ التسليم المتوقع لا يجوز أن يسبق تاريخ الاستلام
        public static bool IsValidDueDate(DateTime receivedDate, DateTime dueDate)
        {
            return Register(IsInRange(dueDate) && dueDate.Date >= receivedDate.Date);
        }// نهاية IsValidDueDate

        // رقم أكبر من الصفر تماما (السعة، الاستطاعة...)
        public static bool IsPositive(double value)
        {
            return Register(value > 0);
        }// نهاية IsPositive

        // رقم يمكن أن يساوي الصفر (ساعات العمل، تكلفة القطع...)
        public static bool IsNotNegative(double value)
        {
            return Register(value >= 0);
        }// نهاية IsNotNegative

        // دالة مساعدة: هل السنة ضمن المجال المقبول؟
        private static bool IsInRange(DateTime date)
        {
            return date.Year >= MinYear && date.Year <= MaxYear;
        }// نهاية IsInRange

        // تعد حالات الفشل وتعيد النتيجة كما هي
        private static bool Register(bool result)
        {
            if (!result) FailedChecks++;
            return result;
        }// نهاية Register
    }// نهاية الفئة Validator
}// نهاية فضاء الأسماء Utilities
