using Pharmacy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.Infrastructure.Persistence;

public static class PharmacyDbContextSeed
{
    public static async Task SeedAsync(PharmacyDbContext context)
    {
        // Seed AppLookupMaster and AppLookupDetail (insert only non-existing)
        await SeedLookupsAsync(context);

        // Seed default roles (insert only non-existing)
        await SeedRolesAsync(context);
    }

    private static async Task SeedLookupsAsync(PharmacyDbContext context)
    {
        var lookupMasters = new List<AppLookupMaster>();
        var lookupDetails = new List<AppLookupDetail>();

        // ====================================
        // 1. IDENTITY TYPE
        // ====================================
        var identityTypeMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            LookupCode = "IDENTITY_TYPE",
            LookupNameAr = "نوع الهوية",
            LookupNameEn = "Identity Type",
            Description = "Patient and User Identity Types",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(identityTypeMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0001-000000000001"),
                MasterID = identityTypeMaster.Oid,
                ValueCode = "NATIONAL_ID",
                ValueNameAr = "الهوية الوطنية",
                ValueNameEn = "National ID",
                SortOrder = 1,
                IsDefault = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0001-000000000002"),
                MasterID = identityTypeMaster.Oid,
                ValueCode = "PASSPORT",
                ValueNameAr = "جواز السفر",
                ValueNameEn = "Passport",
                SortOrder = 2,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0001-000000000003"),
                MasterID = identityTypeMaster.Oid,
                ValueCode = "IQAMA",
                ValueNameAr = "الإقامة",
                ValueNameEn = "Iqama (Residency)",
                SortOrder = 3,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        });

        // ====================================
        // 2. GENDER
        // ====================================
        var genderMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("10000000-0000-0000-0000-000000000002"),
            LookupCode = "GENDER",
            LookupNameAr = "الجنس",
            LookupNameEn = "Gender",
            Description = "Patient and User Gender",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(genderMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0002-000000000001"),
                MasterID = genderMaster.Oid,
                ValueCode = "MALE",
                ValueNameAr = "ذكر",
                ValueNameEn = "Male",
                SortOrder = 1,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0002-000000000002"),
                MasterID = genderMaster.Oid,
                ValueCode = "FEMALE",
                ValueNameAr = "أنثى",
                ValueNameEn = "Female",
                SortOrder = 2,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        });

        // ====================================
        // 3. MARITAL STATUS
        // ====================================
        var maritalStatusMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("10000000-0000-0000-0000-000000000003"),
            LookupCode = "MARITAL_STATUS",
            LookupNameAr = "الحالة الاجتماعية",
            LookupNameEn = "Marital Status",
            Description = "Patient Marital Status",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(maritalStatusMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0003-000000000001"),
                MasterID = maritalStatusMaster.Oid,
                ValueCode = "SINGLE",
                ValueNameAr = "أعزب",
                ValueNameEn = "Single",
                SortOrder = 1,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0003-000000000002"),
                MasterID = maritalStatusMaster.Oid,
                ValueCode = "MARRIED",
                ValueNameAr = "متزوج",
                ValueNameEn = "Married",
                SortOrder = 2,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0003-000000000003"),
                MasterID = maritalStatusMaster.Oid,
                ValueCode = "DIVORCED",
                ValueNameAr = "مطلق",
                ValueNameEn = "Divorced",
                SortOrder = 3,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0003-000000000004"),
                MasterID = maritalStatusMaster.Oid,
                ValueCode = "WIDOWED",
                ValueNameAr = "أرمل",
                ValueNameEn = "Widowed",
                SortOrder = 4,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        });

        // ====================================
        // 4. NATIONALITY
        // ====================================
        var nationalityMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("10000000-0000-0000-0000-000000000004"),
            LookupCode = "NATIONALITY",
            LookupNameAr = "الجنسية",
            LookupNameEn = "Nationality",
            Description = "Patient Nationality",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(nationalityMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0004-000000000001"),
                MasterID = nationalityMaster.Oid,
                ValueCode = "SA",
                ValueNameAr = "سعودي",
                ValueNameEn = "Saudi",
                SortOrder = 1,
                IsDefault = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0004-000000000002"),
                MasterID = nationalityMaster.Oid,
                ValueCode = "EG",
                ValueNameAr = "مصري",
                ValueNameEn = "Egyptian",
                SortOrder = 2,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0004-000000000003"),
                MasterID = nationalityMaster.Oid,
                ValueCode = "JO",
                ValueNameAr = "أردني",
                ValueNameEn = "Jordanian",
                SortOrder = 3,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0004-000000000004"),
                MasterID = nationalityMaster.Oid,
                ValueCode = "YE",
                ValueNameAr = "يمني",
                ValueNameEn = "Yemeni",
                SortOrder = 4,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0004-000000000005"),
                MasterID = nationalityMaster.Oid,
                ValueCode = "SY",
                ValueNameAr = "سوري",
                ValueNameEn = "Syrian",
                SortOrder = 5,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0004-000000000006"),
                MasterID = nationalityMaster.Oid,
                ValueCode = "OTHER",
                ValueNameAr = "أخرى",
                ValueNameEn = "Other",
                SortOrder = 99,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        });

        // ====================================
        // 5. BLOOD GROUP
        // ====================================
        var bloodGroupMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            LookupCode = "BLOOD_GROUP",
            LookupNameEn = "Blood Group",
            LookupNameAr = "فصيلة الدم",
            Description = "Patient Blood Groups",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(bloodGroupMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0005-000000000001"),
                MasterID = bloodGroupMaster.Oid,
                ValueCode = "A_POSITIVE",
                ValueNameAr = "A+",
                ValueNameEn = "A+",
                SortOrder = 1,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0005-000000000002"),
                MasterID = bloodGroupMaster.Oid,
                ValueCode = "A_NEGATIVE",
                ValueNameAr = "A-",
                ValueNameEn = "A-",
                SortOrder = 2,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0005-000000000003"),
                MasterID = bloodGroupMaster.Oid,
                ValueCode = "B_POSITIVE",
                ValueNameAr = "B+",
                ValueNameEn = "B+",
                SortOrder = 3,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0005-000000000004"),
                MasterID = bloodGroupMaster.Oid,
                ValueCode = "B_NEGATIVE",
                ValueNameAr = "B-",
                ValueNameEn = "B-",
                SortOrder = 4,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0005-000000000005"),
                MasterID = bloodGroupMaster.Oid,
                ValueCode = "AB_POSITIVE",
                ValueNameAr = "AB+",
                ValueNameEn = "AB+",
                SortOrder = 5,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0005-000000000006"),
                MasterID = bloodGroupMaster.Oid,
                ValueCode = "AB_NEGATIVE",
                ValueNameAr = "AB-",
                ValueNameEn = "AB-",
                SortOrder = 6,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0005-000000000007"),
                MasterID = bloodGroupMaster.Oid,
                ValueCode = "O_POSITIVE",
                ValueNameAr = "O+",
                ValueNameEn = "O+",
                SortOrder = 7,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0005-000000000008"),
                MasterID = bloodGroupMaster.Oid,
                ValueCode = "O_NEGATIVE",
                ValueNameAr = "O-",
                ValueNameEn = "O-",
                SortOrder = 8,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        });

        // ====================================
        // 6. DEPARTMENT
        // ====================================
        var departmentMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("10000000-0000-0000-0000-000000000006"),
            LookupCode = "DEPARTMENT",
            LookupNameEn = "Department",
            LookupNameAr = "القسم",
            Description = "Pharmacy Departments",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(departmentMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0006-000000000001"),
                MasterID = departmentMaster.Oid,
                ValueCode = "EMERGENCY",
                ValueNameAr = "الطوارئ",
                ValueNameEn = "Emergency",
                SortOrder = 1,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0006-000000000002"),
                MasterID = departmentMaster.Oid,
                ValueCode = "ICU",
                ValueNameAr = "العناية المركزة",
                ValueNameEn = "Intensive Care Unit",
                SortOrder = 2,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0006-000000000003"),
                MasterID = departmentMaster.Oid,
                ValueCode = "SURGERY",
                ValueNameAr = "الجراحة",
                ValueNameEn = "Surgery",
                SortOrder = 3,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0006-000000000004"),
                MasterID = departmentMaster.Oid,
                ValueCode = "PEDIATRICS",
                ValueNameAr = "طب الأطفال",
                ValueNameEn = "Pediatrics",
                SortOrder = 4,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0006-000000000005"),
                MasterID = departmentMaster.Oid,
                ValueCode = "OB_GYN",
                ValueNameAr = "النساء والتوليد",
                ValueNameEn = "Obstetrics & Gynecology",
                SortOrder = 5,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0006-000000000006"),
                MasterID = departmentMaster.Oid,
                ValueCode = "CARDIOLOGY",
                ValueNameAr = "أمراض القلب",
                ValueNameEn = "Cardiology",
                SortOrder = 6,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0006-000000000007"),
                MasterID = departmentMaster.Oid,
                ValueCode = "ORTHOPEDICS",
                ValueNameAr = "جراحة العظام",
                ValueNameEn = "Orthopedics",
                SortOrder = 7,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0006-000000000008"),
                MasterID = departmentMaster.Oid,
                ValueCode = "RADIOLOGY",
                ValueNameAr = "الأشعة",
                ValueNameEn = "Radiology",
                SortOrder = 8,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0006-000000000009"),
                MasterID = departmentMaster.Oid,
                ValueCode = "LABORATORY",
                ValueNameAr = "المختبر",
                ValueNameEn = "Laboratory",
                SortOrder = 9,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0006-000000000010"),
                MasterID = departmentMaster.Oid,
                ValueCode = "PHARMACY",
                ValueNameAr = "الصيدلية",
                ValueNameEn = "Pharmacy",
                SortOrder = 10,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        });

        // ====================================
        // 7. APPOINTMENT STATUS
        // ====================================
        var appointmentStatusMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("10000000-0000-0000-0000-000000000007"),
            LookupCode = "APPOINTMENT_STATUS",
            LookupNameAr = "حالة الموعد",
            LookupNameEn = "Appointment Status",
            Description = "Appointment Status Types",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(appointmentStatusMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0007-000000000001"),
                MasterID = appointmentStatusMaster.Oid,
                ValueCode = "SCHEDULED",
                ValueNameAr = "مجدول",
                ValueNameEn = "Scheduled",
                SortOrder = 1,
                IsDefault = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0007-000000000002"),
                MasterID = appointmentStatusMaster.Oid,
                ValueCode = "CONFIRMED",
                ValueNameAr = "مؤكد",
                ValueNameEn = "Confirmed",
                SortOrder = 2,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0007-000000000003"),
                MasterID = appointmentStatusMaster.Oid,
                ValueCode = "IN_PROGRESS",
                ValueNameAr = "قيد التنفيذ",
                ValueNameEn = "In Progress",
                SortOrder = 3,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0007-000000000004"),
                MasterID = appointmentStatusMaster.Oid,
                ValueCode = "COMPLETED",
                ValueNameAr = "مكتمل",
                ValueNameEn = "Completed",
                SortOrder = 4,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0007-000000000005"),
                MasterID = appointmentStatusMaster.Oid,
                ValueCode = "CANCELLED",
                ValueNameAr = "ملغي",
                ValueNameEn = "Cancelled",
                SortOrder = 5,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0007-000000000006"),
                MasterID = appointmentStatusMaster.Oid,
                ValueCode = "NO_SHOW",
                ValueNameAr = "لم يحضر",
                ValueNameEn = "No Show",
                SortOrder = 6,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        });

        // ====================================
        // 8. ENCOUNTER TYPE
        // ====================================
        var encounterTypeMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            LookupCode = "ENCOUNTER_TYPE",
            LookupNameAr = "نوع اللقاء",
            LookupNameEn = "Encounter Type",
            Description = "Encounter Types",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(encounterTypeMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0008-000000000001"),
                MasterID = encounterTypeMaster.Oid,
                ValueCode = "OUTPATIENT",
                ValueNameAr = "عيادات خارجية",
                ValueNameEn = "Outpatient",
                SortOrder = 1,
                IsDefault = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0008-000000000002"),
                MasterID = encounterTypeMaster.Oid,
                ValueCode = "INPATIENT",
                ValueNameAr = "مرضى داخليين",
                ValueNameEn = "Inpatient",
                SortOrder = 2,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0008-000000000003"),
                MasterID = encounterTypeMaster.Oid,
                ValueCode = "EMERGENCY",
                ValueNameAr = "طوارئ",
                ValueNameEn = "Emergency",
                SortOrder = 3,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new AppLookupDetail
            {
                Oid = Guid.Parse("10000000-0000-0000-0008-000000000004"),
                MasterID = encounterTypeMaster.Oid,
                ValueCode = "FOLLOW_UP",
                ValueNameAr = "متابعة",
                ValueNameEn = "Follow-up",
                SortOrder = 4,
                IsDefault = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        });

        // ====================================
        // 9. STAKEHOLDER_TYPE - Stakeholder.StakeholderTypeId
        // ====================================
        var stakeholderTypeMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111003"),
            LookupCode = "STAKEHOLDER_TYPE",
            LookupNameAr = "نوع الجهة",
            LookupNameEn = "Stakeholder Type",
            Description = "Types of external entities (Pharmacy, Supplier, Distributor, etc.)",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(stakeholderTypeMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222010"), MasterID = stakeholderTypeMaster.Oid, ValueCode = "PHARMACY", ValueNameAr = "صيدلية", ValueNameEn = "Pharmacy", SortOrder = 1, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222011"), MasterID = stakeholderTypeMaster.Oid, ValueCode = "SUPPLIER", ValueNameAr = "مورد", ValueNameEn = "Supplier", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222012"), MasterID = stakeholderTypeMaster.Oid, ValueCode = "DISTRIBUTOR", ValueNameAr = "موزع", ValueNameEn = "Distributor", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222013"), MasterID = stakeholderTypeMaster.Oid, ValueCode = "MANUFACTURER", ValueNameAr = "مصنع", ValueNameEn = "Manufacturer", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222014"), MasterID = stakeholderTypeMaster.Oid, ValueCode = "WHOLESALER", ValueNameAr = "تاجر جملة", ValueNameEn = "Wholesaler", SortOrder = 5, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 10. PRODUCT_TYPE - Product.ProductTypeId
        // ====================================
        var productTypeMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111004"),
            LookupCode = "PRODUCT_TYPE",
            LookupNameAr = "نوع المنتج",
            LookupNameEn = "Product Type",
            Description = "Pharmaceutical product dosage forms",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(productTypeMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222020"), MasterID = productTypeMaster.Oid, ValueCode = "TABLET", ValueNameAr = "أقراص", ValueNameEn = "Tablet", SortOrder = 1, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222021"), MasterID = productTypeMaster.Oid, ValueCode = "SYRUP", ValueNameAr = "شراب", ValueNameEn = "Syrup", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222022"), MasterID = productTypeMaster.Oid, ValueCode = "INJECTION", ValueNameAr = "حقن", ValueNameEn = "Injection", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222023"), MasterID = productTypeMaster.Oid, ValueCode = "CAPSULE", ValueNameAr = "كبسولات", ValueNameEn = "Capsule", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222024"), MasterID = productTypeMaster.Oid, ValueCode = "OINTMENT", ValueNameAr = "مرهم", ValueNameEn = "Ointment", SortOrder = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222025"), MasterID = productTypeMaster.Oid, ValueCode = "CREAM", ValueNameAr = "كريم", ValueNameEn = "Cream", SortOrder = 6, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222026"), MasterID = productTypeMaster.Oid, ValueCode = "DROPS", ValueNameAr = "قطرة", ValueNameEn = "Drops", SortOrder = 7, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222027"), MasterID = productTypeMaster.Oid, ValueCode = "SUPPOSITORY", ValueNameAr = "تحاميل", ValueNameEn = "Suppository", SortOrder = 8, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222028"), MasterID = productTypeMaster.Oid, ValueCode = "POWDER", ValueNameAr = "بودرة", ValueNameEn = "Powder", SortOrder = 9, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222029"), MasterID = productTypeMaster.Oid, ValueCode = "INHALER", ValueNameAr = "بخاخ", ValueNameEn = "Inhaler", SortOrder = 10, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 11. TRANSACTION_TYPE - StockTransaction.TransactionTypeId
        // ====================================
        var transactionTypeMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111005"),
            LookupCode = "TRANSACTION_TYPE",
            LookupNameAr = "نوع المعاملة",
            LookupNameEn = "Transaction Type",
            Description = "Stock transaction types (IN, OUT, TRANSFER, etc.)",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(transactionTypeMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222030"), MasterID = transactionTypeMaster.Oid, ValueCode = "IN", ValueNameAr = "وارد", ValueNameEn = "Stock In", SortOrder = 1, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222031"), MasterID = transactionTypeMaster.Oid, ValueCode = "OUT", ValueNameAr = "صادر", ValueNameEn = "Stock Out", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222032"), MasterID = transactionTypeMaster.Oid, ValueCode = "TRANSFER", ValueNameAr = "تحويل", ValueNameEn = "Transfer", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222033"), MasterID = transactionTypeMaster.Oid, ValueCode = "ADJUSTMENT", ValueNameAr = "تسوية", ValueNameEn = "Adjustment", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222034"), MasterID = transactionTypeMaster.Oid, ValueCode = "RETURN", ValueNameAr = "مرتجع", ValueNameEn = "Return", SortOrder = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222035"), MasterID = transactionTypeMaster.Oid, ValueCode = "EXPIRED", ValueNameAr = "منتهي الصلاحية", ValueNameEn = "Expired", SortOrder = 6, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222036"), MasterID = transactionTypeMaster.Oid, ValueCode = "DAMAGED", ValueNameAr = "تالف", ValueNameEn = "Damaged", SortOrder = 7, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 12. PAYMENT_METHOD - SalesInvoice.PaymentMethodId
        // ====================================
        var paymentMethodMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111006"),
            LookupCode = "PAYMENT_METHOD",
            LookupNameAr = "طريقة الدفع",
            LookupNameEn = "Payment Method",
            Description = "Payment methods for sales transactions",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(paymentMethodMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222040"), MasterID = paymentMethodMaster.Oid, ValueCode = "CASH", ValueNameAr = "نقدي", ValueNameEn = "Cash", SortOrder = 1, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222041"), MasterID = paymentMethodMaster.Oid, ValueCode = "CARD", ValueNameAr = "بطاقة", ValueNameEn = "Card", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222042"), MasterID = paymentMethodMaster.Oid, ValueCode = "CREDIT", ValueNameAr = "آجل", ValueNameEn = "Credit", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222043"), MasterID = paymentMethodMaster.Oid, ValueCode = "INSURANCE", ValueNameAr = "تأمين", ValueNameEn = "Insurance", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222044"), MasterID = paymentMethodMaster.Oid, ValueCode = "MIXED", ValueNameAr = "مختلط", ValueNameEn = "Mixed", SortOrder = 5, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 13. INVOICE_STATUS - SalesInvoice.InvoiceStatusId
        // ====================================
        var invoiceStatusMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111007"),
            LookupCode = "INVOICE_STATUS",
            LookupNameAr = "حالة الفاتورة",
            LookupNameEn = "Invoice Status",
            Description = "Sales invoice status options",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(invoiceStatusMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222050"), MasterID = invoiceStatusMaster.Oid, ValueCode = "PENDING", ValueNameAr = "معلق", ValueNameEn = "Pending", SortOrder = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222051"), MasterID = invoiceStatusMaster.Oid, ValueCode = "COMPLETED", ValueNameAr = "مكتمل", ValueNameEn = "Completed", SortOrder = 2, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222052"), MasterID = invoiceStatusMaster.Oid, ValueCode = "CANCELLED", ValueNameAr = "ملغي", ValueNameEn = "Cancelled", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222053"), MasterID = invoiceStatusMaster.Oid, ValueCode = "REFUNDED", ValueNameAr = "مسترد", ValueNameEn = "Refunded", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222054"), MasterID = invoiceStatusMaster.Oid, ValueCode = "PARTIAL_REFUND", ValueNameAr = "استرداد جزئي", ValueNameEn = "Partial Refund", SortOrder = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222055"), MasterID = invoiceStatusMaster.Oid, ValueCode = "ON_HOLD", ValueNameAr = "معلق", ValueNameEn = "On Hold", SortOrder = 6, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 14. BATCH_STATUS - ProductBatch.BatchStatusId
        // ====================================
        var batchStatusMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111008"),
            LookupCode = "BATCH_STATUS",
            LookupNameAr = "حالة الدفعة",
            LookupNameEn = "Batch Status",
            Description = "Product batch status options",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(batchStatusMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222060"), MasterID = batchStatusMaster.Oid, ValueCode = "ACTIVE", ValueNameAr = "نشط", ValueNameEn = "Active", SortOrder = 1, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222061"), MasterID = batchStatusMaster.Oid, ValueCode = "EXPIRED", ValueNameAr = "منتهي", ValueNameEn = "Expired", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222062"), MasterID = batchStatusMaster.Oid, ValueCode = "QUARANTINE", ValueNameAr = "حجر", ValueNameEn = "Quarantine", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222063"), MasterID = batchStatusMaster.Oid, ValueCode = "DEPLETED", ValueNameAr = "مستنفد", ValueNameEn = "Depleted", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222064"), MasterID = batchStatusMaster.Oid, ValueCode = "RECALLED", ValueNameAr = "مسحوب", ValueNameEn = "Recalled", SortOrder = 5, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 15. DRUG_STATUS - Product drug registration status
        // ====================================
        var drugStatusMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111009"),
            LookupCode = "DRUG_STATUS",
            LookupNameAr = "حالة الدواء",
            LookupNameEn = "Drug Status",
            Description = "Drug registration/marketing status",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(drugStatusMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222070"), MasterID = drugStatusMaster.Oid, ValueCode = "ACTIVE", ValueNameAr = "نشط", ValueNameEn = "Active", SortOrder = 1, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222071"), MasterID = drugStatusMaster.Oid, ValueCode = "DISCONTINUED", ValueNameAr = "متوقف", ValueNameEn = "Discontinued", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222072"), MasterID = drugStatusMaster.Oid, ValueCode = "PENDING", ValueNameAr = "قيد المراجعة", ValueNameEn = "Pending", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222073"), MasterID = drugStatusMaster.Oid, ValueCode = "SUSPENDED", ValueNameAr = "موقوف", ValueNameEn = "Suspended", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 16. LEGAL_STATUS - Product legal classification
        // ====================================
        var legalStatusMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111010"),
            LookupCode = "LEGAL_STATUS",
            LookupNameAr = "التصنيف القانوني",
            LookupNameEn = "Legal Status",
            Description = "Drug legal classification (OTC, Prescription, Controlled)",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(legalStatusMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222080"), MasterID = legalStatusMaster.Oid, ValueCode = "OTC", ValueNameAr = "بدون وصفة", ValueNameEn = "Over The Counter", SortOrder = 1, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222081"), MasterID = legalStatusMaster.Oid, ValueCode = "PRESCRIPTION", ValueNameAr = "بوصفة طبية", ValueNameEn = "Prescription", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222082"), MasterID = legalStatusMaster.Oid, ValueCode = "CONTROLLED", ValueNameAr = "خاضع للرقابة", ValueNameEn = "Controlled", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222083"), MasterID = legalStatusMaster.Oid, ValueCode = "NARCOTIC", ValueNameAr = "مخدر", ValueNameEn = "Narcotic", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 17. PACKAGE_TYPE - Product.PackageTypeId, ProductUnit.PackageTypeId
        // ====================================
        var packageTypeMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111011"),
            LookupCode = "PACKAGE_TYPE",
            LookupNameAr = "نوع العبوة",
            LookupNameEn = "Package Type",
            Description = "Product packaging types",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(packageTypeMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222090"), MasterID = packageTypeMaster.Oid, ValueCode = "BOX", ValueNameAr = "علبة", ValueNameEn = "Box", SortOrder = 1, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222091"), MasterID = packageTypeMaster.Oid, ValueCode = "BOTTLE", ValueNameAr = "زجاجة", ValueNameEn = "Bottle", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222092"), MasterID = packageTypeMaster.Oid, ValueCode = "BLISTER", ValueNameAr = "شريط", ValueNameEn = "Blister", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222093"), MasterID = packageTypeMaster.Oid, ValueCode = "TUBE", ValueNameAr = "أنبوب", ValueNameEn = "Tube", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222094"), MasterID = packageTypeMaster.Oid, ValueCode = "VIAL", ValueNameAr = "قارورة", ValueNameEn = "Vial", SortOrder = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222095"), MasterID = packageTypeMaster.Oid, ValueCode = "AMPOULE", ValueNameAr = "أمبولة", ValueNameEn = "Ampoule", SortOrder = 6, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-222222222096"), MasterID = packageTypeMaster.Oid, ValueCode = "SACHET", ValueNameAr = "كيس", ValueNameEn = "Sachet", SortOrder = 7, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 18. STRENGTH_UNIT - Product strength measurement units
        // ====================================
        var strengthUnitMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111012"),
            LookupCode = "STRENGTH_UNIT",
            LookupNameAr = "وحدة التركيز",
            LookupNameEn = "Strength Unit",
            Description = "Drug strength measurement units",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(strengthUnitMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220A0"), MasterID = strengthUnitMaster.Oid, ValueCode = "MG", ValueNameAr = "مليغرام", ValueNameEn = "Milligram (mg)", SortOrder = 1, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220A1"), MasterID = strengthUnitMaster.Oid, ValueCode = "G", ValueNameAr = "غرام", ValueNameEn = "Gram (g)", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220A2"), MasterID = strengthUnitMaster.Oid, ValueCode = "ML", ValueNameAr = "مليلتر", ValueNameEn = "Milliliter (ml)", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220A3"), MasterID = strengthUnitMaster.Oid, ValueCode = "L", ValueNameAr = "لتر", ValueNameEn = "Liter (L)", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220A4"), MasterID = strengthUnitMaster.Oid, ValueCode = "IU", ValueNameAr = "وحدة دولية", ValueNameEn = "International Unit (IU)", SortOrder = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220A5"), MasterID = strengthUnitMaster.Oid, ValueCode = "PERCENT", ValueNameAr = "نسبة مئوية", ValueNameEn = "Percent (%)", SortOrder = 6, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220A6"), MasterID = strengthUnitMaster.Oid, ValueCode = "MCG", ValueNameAr = "ميكروغرام", ValueNameEn = "Microgram (mcg)", SortOrder = 7, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 19. RSD_OPERATION_TYPE - RsdOperationLog.OperationTypeId
        // ====================================
        var rsdOperationTypeMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111013"),
            LookupCode = "RSD_OPERATION_TYPE",
            LookupNameAr = "نوع عملية RSD",
            LookupNameEn = "RSD Operation Type",
            Description = "Types of SFDA RSD integration operations",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(rsdOperationTypeMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220B0"), MasterID = rsdOperationTypeMaster.Oid, ValueCode = "ACCEPT_BATCH", ValueNameAr = "قبول دفعة", ValueNameEn = "Accept Batch", SortOrder = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220B1"), MasterID = rsdOperationTypeMaster.Oid, ValueCode = "PHARMACY_SALE", ValueNameAr = "بيع صيدلية", ValueNameEn = "Pharmacy Sale", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220B2"), MasterID = rsdOperationTypeMaster.Oid, ValueCode = "PHARMACY_SALE_CANCEL", ValueNameAr = "إلغاء بيع صيدلية", ValueNameEn = "Pharmacy Sale Cancel", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220B3"), MasterID = rsdOperationTypeMaster.Oid, ValueCode = "RETURN_BATCH", ValueNameAr = "إرجاع دفعة", ValueNameEn = "Return Batch", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 20. VAT_TYPE - Product.VatTypeId
        // ====================================
        var vatTypeMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111014"),
            LookupCode = "VAT_TYPE",
            LookupNameAr = "نوع الضريبة",
            LookupNameEn = "VAT Type",
            Description = "Value Added Tax types for products",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(vatTypeMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220C0"), MasterID = vatTypeMaster.Oid, ValueCode = "STANDARD", ValueNameAr = "ضريبة قياسية (15%)", ValueNameEn = "Standard (15%)", SortOrder = 1, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220C1"), MasterID = vatTypeMaster.Oid, ValueCode = "ZERO_RATED", ValueNameAr = "نسبة صفر", ValueNameEn = "Zero Rated (0%)", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220C2"), MasterID = vatTypeMaster.Oid, ValueCode = "EXEMPT", ValueNameAr = "معفى", ValueNameEn = "Exempt", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 21. PRODUCT_GROUP - Product.ProductGroupId
        // ====================================
        var productGroupMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111015"),
            LookupCode = "PRODUCT_GROUP",
            LookupNameAr = "مجموعة المنتج",
            LookupNameEn = "Product Group",
            Description = "Product category groups",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(productGroupMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220D0"), MasterID = productGroupMaster.Oid, ValueCode = "PRESCRIPTION", ValueNameAr = "أدوية وصفية", ValueNameEn = "Prescription Drugs", SortOrder = 1, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220D1"), MasterID = productGroupMaster.Oid, ValueCode = "OTC", ValueNameAr = "أدوية بدون وصفة", ValueNameEn = "OTC Medications", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220D2"), MasterID = productGroupMaster.Oid, ValueCode = "MEDICAL_DEVICE", ValueNameAr = "أجهزة طبية", ValueNameEn = "Medical Devices", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220D3"), MasterID = productGroupMaster.Oid, ValueCode = "SUPPLEMENT", ValueNameAr = "مكملات غذائية", ValueNameEn = "Supplements & Vitamins", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220D4"), MasterID = productGroupMaster.Oid, ValueCode = "PERSONAL_CARE", ValueNameAr = "عناية شخصية", ValueNameEn = "Personal Care", SortOrder = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220D5"), MasterID = productGroupMaster.Oid, ValueCode = "COSMETIC", ValueNameAr = "مستحضرات تجميل", ValueNameEn = "Cosmetics", SortOrder = 6, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 22. DOSAGE_FORM - Product.DosageFormId
        // ====================================
        var dosageFormMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111016"),
            LookupCode = "DOSAGE_FORM",
            LookupNameAr = "الشكل الصيدلاني",
            LookupNameEn = "Dosage Form",
            Description = "Pharmaceutical dosage forms",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(dosageFormMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220E0"), MasterID = dosageFormMaster.Oid, ValueCode = "TABLET", ValueNameAr = "أقراص", ValueNameEn = "Tablet", SortOrder = 1, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220E1"), MasterID = dosageFormMaster.Oid, ValueCode = "CAPSULE", ValueNameAr = "كبسولات", ValueNameEn = "Capsule", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220E2"), MasterID = dosageFormMaster.Oid, ValueCode = "SYRUP", ValueNameAr = "شراب", ValueNameEn = "Syrup", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220E3"), MasterID = dosageFormMaster.Oid, ValueCode = "SUSPENSION", ValueNameAr = "معلق", ValueNameEn = "Suspension", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220E4"), MasterID = dosageFormMaster.Oid, ValueCode = "SOLUTION", ValueNameAr = "محلول", ValueNameEn = "Solution", SortOrder = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220E5"), MasterID = dosageFormMaster.Oid, ValueCode = "INJECTION", ValueNameAr = "حقن", ValueNameEn = "Injection", SortOrder = 6, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220E6"), MasterID = dosageFormMaster.Oid, ValueCode = "OINTMENT", ValueNameAr = "مرهم", ValueNameEn = "Ointment", SortOrder = 7, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220E7"), MasterID = dosageFormMaster.Oid, ValueCode = "CREAM", ValueNameAr = "كريم", ValueNameEn = "Cream", SortOrder = 8, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220E8"), MasterID = dosageFormMaster.Oid, ValueCode = "GEL", ValueNameAr = "جل", ValueNameEn = "Gel", SortOrder = 9, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220E9"), MasterID = dosageFormMaster.Oid, ValueCode = "DROPS", ValueNameAr = "قطرة", ValueNameEn = "Drops", SortOrder = 10, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220EA"), MasterID = dosageFormMaster.Oid, ValueCode = "SPRAY", ValueNameAr = "بخاخ", ValueNameEn = "Spray", SortOrder = 11, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220EB"), MasterID = dosageFormMaster.Oid, ValueCode = "INHALER", ValueNameAr = "جهاز استنشاق", ValueNameEn = "Inhaler", SortOrder = 12, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220EC"), MasterID = dosageFormMaster.Oid, ValueCode = "SUPPOSITORY", ValueNameAr = "تحاميل", ValueNameEn = "Suppository", SortOrder = 13, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220ED"), MasterID = dosageFormMaster.Oid, ValueCode = "PATCH", ValueNameAr = "لصقة", ValueNameEn = "Patch", SortOrder = 14, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220EE"), MasterID = dosageFormMaster.Oid, ValueCode = "POWDER", ValueNameAr = "بودرة", ValueNameEn = "Powder", SortOrder = 15, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // ====================================
        // 23. RETURN_REASON - ReturnInvoice.ReturnReasonId
        // ====================================
        var returnReasonMaster = new AppLookupMaster
        {
            Oid = Guid.Parse("11111111-1111-1111-1111-111111111017"),
            LookupCode = "RETURN_REASON",
            LookupNameAr = "سبب الإرجاع",
            LookupNameEn = "Return Reason",
            Description = "Reasons for product return/refund",
            CreatedAt = DateTime.UtcNow
        };
        lookupMasters.Add(returnReasonMaster);

        lookupDetails.AddRange(new[]
        {
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220F0"), MasterID = returnReasonMaster.Oid, ValueCode = "DEFECTIVE", ValueNameAr = "منتج معيب", ValueNameEn = "Defective Product", SortOrder = 1, IsDefault = true, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220F1"), MasterID = returnReasonMaster.Oid, ValueCode = "EXPIRED", ValueNameAr = "منتهي الصلاحية", ValueNameEn = "Expired Product", SortOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220F2"), MasterID = returnReasonMaster.Oid, ValueCode = "WRONG_PRODUCT", ValueNameAr = "منتج خاطئ", ValueNameEn = "Wrong Product Dispensed", SortOrder = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220F3"), MasterID = returnReasonMaster.Oid, ValueCode = "CUSTOMER_REQUEST", ValueNameAr = "طلب العميل", ValueNameEn = "Customer Request", SortOrder = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220F4"), MasterID = returnReasonMaster.Oid, ValueCode = "ADVERSE_REACTION", ValueNameAr = "تفاعل عكسي", ValueNameEn = "Adverse Reaction", SortOrder = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
            new AppLookupDetail { Oid = Guid.Parse("22222222-2222-2222-2222-2222222220F5"), MasterID = returnReasonMaster.Oid, ValueCode = "OTHER", ValueNameAr = "أخرى", ValueNameEn = "Other", SortOrder = 99, IsActive = true, CreatedAt = DateTime.UtcNow }
        });

        // Save only what doesn't already exist (upsert by Oid)
        var existingMasterIds = (await context.AppLookupMasters
            .Select(m => m.Oid)
            .ToListAsync())
            .ToHashSet();

        var newMasters = lookupMasters.Where(m => !existingMasterIds.Contains(m.Oid)).ToList();
        if (newMasters.Count > 0)
        {
            context.AppLookupMasters.AddRange(newMasters);
            await context.SaveChangesAsync();
        }

        var existingDetailIds = (await context.AppLookupDetails
            .Select(d => d.Oid)
            .ToListAsync())
            .ToHashSet();

        var newDetails = lookupDetails.Where(d => !existingDetailIds.Contains(d.Oid)).ToList();
        if (newDetails.Count > 0)
        {
            context.AppLookupDetails.AddRange(newDetails);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedRolesAsync(PharmacyDbContext context)
    {
        var roles = new[]
        {
            new Role
            {
                Oid = Guid.Parse("20000000-0000-0000-0000-000000000001"),
                Name = "Admin",
                Description = "System Administrator with full access",
                CreatedAt = DateTime.UtcNow
            },
            new Role
            {
                Oid = Guid.Parse("20000000-0000-0000-0000-000000000002"),
                Name = "Doctor",
                Description = "Medical Doctor",
                CreatedAt = DateTime.UtcNow
            },
            new Role
            {
                Oid = Guid.Parse("20000000-0000-0000-0000-000000000003"),
                Name = "Nurse",
                Description = "Nursing Staff",
                CreatedAt = DateTime.UtcNow
            },
            new Role
            {
                Oid = Guid.Parse("20000000-0000-0000-0000-000000000004"),
                Name = "Receptionist",
                Description = "Front Desk and Reception",
                CreatedAt = DateTime.UtcNow
            },
            new Role
            {
                Oid = Guid.Parse("20000000-0000-0000-0000-000000000005"),
                Name = "Pharmacist",
                Description = "Pharmacy Staff",
                CreatedAt = DateTime.UtcNow
            },
            new Role
            {
                Oid = Guid.Parse("20000000-0000-0000-0000-000000000006"),
                Name = "Lab Technician",
                Description = "Laboratory Staff",
                CreatedAt = DateTime.UtcNow
            },
            new Role
            {
                Oid = Guid.Parse("20000000-0000-0000-0000-000000000007"),
                Name = "Radiologist",
                Description = "Radiology Staff",
                CreatedAt = DateTime.UtcNow
            }
        };

        var existingRoleIds = (await context.Roles
            .Select(r => r.Oid)
            .ToListAsync())
            .ToHashSet();

        var newRoles = roles.Where(r => !existingRoleIds.Contains(r.Oid)).ToList();
        if (newRoles.Count > 0)
        {
            context.Roles.AddRange(newRoles);
            await context.SaveChangesAsync();
        }
    }
}