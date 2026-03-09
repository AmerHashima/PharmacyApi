using System.Collections.Generic;

namespace Zatca.Models
{
	/// <summary>
	/// Represents the response from a ZATCA CSR compliance or production CSID request.
	/// يمثل الاستجابة من طلب شهادة الامتثال أو الإنتاج من هيئة الزكاة.
	/// </summary>
	public class CsrResponse
	{
		public long RequestID { get; set; }
		public string DispositionMessage { get; set; }
		public string BinarySecurityToken { get; set; }
		public string Secret { get; set; }
		public List<string> Errors { get; set; }
	}
}
