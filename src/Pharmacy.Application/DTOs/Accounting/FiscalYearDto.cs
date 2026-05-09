namespace Pharmacy.Application.DTOs.Accounting;

public class FiscalYearDto
{
    public Guid Oid { get; set; }
    public string NameAR { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsClosed { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class CreateFiscalYearDto
{
    public string NameAR { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsClosed { get; set; } = false;
}

public class UpdateFiscalYearDto
{
    public Guid Oid { get; set; }
    public string NameAR { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsClosed { get; set; }
}
