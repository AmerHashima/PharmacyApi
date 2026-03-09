using System.Collections.Generic;

namespace Zatca.Models
{
	/// <summary>
	/// Represents the validation response returned by the ZATCA portal after invoice submission.
	/// يمثل استجابة التحقق من صحة الفاتورة من بوابة هيئة الزكاة.
	/// </summary>
	public class ZatcaValidationResponse
	{
		public ZatcaValidationResult ValidationResults { get; set; }
		public string ClearanceStatus { get; set; }
		public object ClearedInvoice { get; set; }
		public string ReportingStatus { get; set; }
	}

	/// <summary>
	/// Contains the detailed validation results including errors, warnings, and informational messages.
	/// </summary>
	public class ZatcaValidationResult
	{
		public List<ValidationMessage> ErrorMessages { get; set; }
		public List<ValidationMessage> WarningMessages { get; set; }
		public List<ValidationMessage> InfoMessages { get; set; }
		public string Status { get; set; }
	}

	/// <summary>
	/// A single validation message (error, warning, or info) from the ZATCA portal.
	/// </summary>
	public class ValidationMessage
	{
		public string Type { get; set; }
		public string Code { get; set; }
		public string Category { get; set; }
		public string Message { get; set; }
		public string Status { get; set; }
	}
}
