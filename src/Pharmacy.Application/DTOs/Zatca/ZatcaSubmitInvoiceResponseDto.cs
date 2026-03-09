namespace Pharmacy.Application.DTOs.Zatca;

public class ZatcaSubmitInvoiceResponseDto
{
    public bool Success { get; set; }
    public string? Status { get; set; }
    public string? InvoiceHash { get; set; }
    public string? ClearedInvoice { get; set; }
    public string? ErrorMessage { get; set; }
    public List<ZatcaValidationMessageDto> Warnings { get; set; } = new();
    public List<ZatcaValidationMessageDto> Errors { get; set; } = new();
}

public class ZatcaValidationMessageDto
{
    public string? Type { get; set; }
    public string? Code { get; set; }
    public string? Category { get; set; }
    public string? Message { get; set; }
    public string? Status { get; set; }
}
