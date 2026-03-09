namespace Zatca.Models
{
	/// <summary>
	/// Stores the certificate and key data obtained after CSR creation and production CSID issuance.
	/// يخزن بيانات الشهادة والمفتاح بعد إنشاء طلب الشهادة وإصدار معرف الإنتاج.
	/// </summary>
	public class CertificateResponse
	{
		public int Id { get; set; }
		public string CertificateContent { get; set; }
		public string PrivateKeyContent { get; set; }
		public string BinarySecurityToken { get; set; }
		public string Secret { get; set; }
		public string Pih { get; set; }
	}
}
