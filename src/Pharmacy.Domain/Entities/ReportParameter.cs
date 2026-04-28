using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities
{
    public class ReportParameter
    {
        [Key]
        public Guid Oid { get; set; }
        [ForeignKey(nameof(Link))]
        public Guid? LinksOid { get; set; }
        public string? Name { get; set; }
        public bool? Active { get; set; }
        public int? Type { get; set; }
        public string? DefaultValue { get; set; }

        public virtual Link? Link { get; set; }
    }
}
