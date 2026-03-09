using System.ComponentModel.DataAnnotations;

namespace Zatca.Models
{
	/// <summary>
	/// Configuration settings for CSR generation and ZATCA portal communication.
	/// إعدادات إنشاء طلب توقيع الشهادة والاتصال ببوابة هيئة الزكاة.
	/// </summary>
	public class CsrSetting
	{
		[Key]
		public int Id { get; set; }

		[MaxLength(255)]
		public string NameEn { get; set; }

		[MaxLength(255)]
		public string Category { get; set; }

		[MaxLength(255)]
		public string Address { get; set; }

		/// <summary>
		/// '1' for Production, '2' for Simulation.
		/// </summary>
		[Required(ErrorMessage = "InvoicePortalType is required.")]
		[RegularExpression("^(1|2)$", ErrorMessage = "InvoicePortalType must be '1' (Production) or '2' (Simulation).")]
		public string InvoicePortalType { get; set; }

		[MaxLength(255)]
		public string Otp { get; set; }

		[MaxLength(255)]
		public string VatRegNo { get; set; }

		[MaxLength(255)]
		public string UnitNameEn { get; set; }

		public string EnvironmentType =>
			InvoicePortalType == "2" ? "Simulation" : "Production";

		public string ZatcaLink =>
			InvoicePortalType == "2"
				? "https://gw-apic-gov.gazt.gov.sa/e-invoicing/simulation/"
				: "https://gw-apic-gov.gazt.gov.sa/e-invoicing/core/";
	}
}
