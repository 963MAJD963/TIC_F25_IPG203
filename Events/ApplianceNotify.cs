using HomeApplianceRepair.Models;

namespace HomeApplianceRepair.Events
{
    // التفويض: يُستخدم للإشعار بتغير حالة جهاز.
    // يجب أن يطابق توقيعُه توقيعَ كل دالة تعالج الحدث.
    public delegate void ApplianceNotify(Appliance appliance, string message);
}// نهاية فضاء الأسماء Events
