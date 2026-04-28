using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domain.Entities
{
    public class Link
    {
        [Key]
        public Guid Oid { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? Icon { get; set; }
        public int? Type { get; set; } // 3 = report
        public bool? Active { get; set; }
        public string? Path { get; set; }
        public string? ReportsKey { get; set; }
        public bool? InViewList { get; set; }
        public virtual ICollection<ReportParameter>? ReportParameters { get; set; }
    }
}
