using Pharmacy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.Infrastructure.Persistence;

public static class PharmacyDbContextSeed
{
    public static async Task SeedAsync(PharmacyDbContext context)
    {
        // Seed AppLookupMaster and AppLookupDetail
        if (!context.AppLookupMasters.Any())
        {
            await SeedLookupsAsync(context);
        }

        // Seed default role
        if (!context.Roles.Any())
        {
            await SeedRolesAsync(context);
        }

        await context.SaveChangesAsync();
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

        // Add all to context
        await context.AppLookupMasters.AddRangeAsync(lookupMasters);
        await context.AppLookupDetails.AddRangeAsync(lookupDetails);
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

        await context.Roles.AddRangeAsync(roles);
    }
}