namespace Zatca.Models
{
	/// <summary>
	/// Represents the solution provider (developer) identification settings.
	/// يمثل إعدادات تعريف مزود الحلول (المطور).
	/// </summary>
	public class ProviderSetting
	{
		public string DevelopProviderName { get; set; }
		public string DevelopVersionNumber { get; set; }
		public string DevelopGUID { get; set; }

		public ProviderSetting(string developProviderName, string developVersionNumber, string developGUID)
		{
			DevelopProviderName = developProviderName;
			DevelopVersionNumber = developVersionNumber;
			DevelopGUID = developGUID;
		}
	}
}
