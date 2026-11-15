# نظام إدارة مركز صيانة وإصلاح الأجهزة المنزلية (C#)

المقرر: البرمجة والتصميم غرضية التوجه (TIC_IPG203_F25) - الجامعة الافتراضية السورية.

تطبيق Console يدير أجهزة منزلية في مركز صيانة (براد، غسالة، مكيف)، ويتابع حالة كل جهاز
(Received ← InRepair ← Repaired ← Delivered) وتكلفة إصلاحه.

## مبادئ البرمجة غرضية التوجه في المشروع
| المبدأ | مكانه في الكود |
|---|---|
| التجريد | الواجهة `Interfaces/IRepairable.cs` والفئة المجردة `Models/Appliance.cs` (الدالة المجردة `CalculateRepairCost`) |
| الوراثة وتعدد الأشكال | `Refrigerator` و `WashingMachine` و `AirConditioner`، والقائمة `List<Appliance>` في `Services/RepairCenter.cs` |
| التغليف | حقول خاصة + خصائص، و `SerialNumber` و `ReceivedDate` و `Status` للقراءة فقط |
| التفويض والأحداث | `Events/ApplianceNotify.cs` والحدثان `StatusChanged` و `DelayDetected` |
| الأعضاء الثابتة | الفئة الثابتة `Utilities/Validator.cs` والخصائص الثابتة في `Appliance` |

## التشغيل
من سطر الأوامر داخل مجلد المشروع:
```
dotnet run
```
أو من Visual Studio: افتح الملف `HomeApplianceRepair.sln` ثم اضغط F5.

يتطلب .NET 8 SDK. التقرير الكامل ومخطط UML في `docs/Report.pdf` (ونسخة Word في `docs/Report.docx`).
