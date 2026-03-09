namespace Zatca.Enums
{
	/// <summary>
	/// ZATCA tax exemption reason codes as defined by the Saudi tax authority.
	/// </summary>
	public enum ZatcaTaxExemptionReasonCode
	{
		/// <summary>Financial services mentioned in Article 29 of the VAT Regulations.</summary>
		VATEX_SA_29,

		/// <summary>Life insurance services mentioned in Article 29 of the VAT Regulations.</summary>
		VATEX_SA_29_7,

		/// <summary>Real estate transactions mentioned in Article 30 of the VAT Regulations.</summary>
		VATEX_SA_30,

		/// <summary>Export of goods.</summary>
		VATEX_SA_32,

		/// <summary>Export of services.</summary>
		VATEX_SA_33,

		/// <summary>The international transport of goods.</summary>
		VATEX_SA_34_1,

		/// <summary>International transport of passengers.</summary>
		VATEX_SA_34_2,

		/// <summary>Services directly connected and incidental to a supply of international passenger transport.</summary>
		VATEX_SA_34_3,

		/// <summary>Supply of a qualifying means of transport.</summary>
		VATEX_SA_34_4,

		/// <summary>Any services relating to goods or passenger transportation, as defined in Article 25.</summary>
		VATEX_SA_34_5,

		/// <summary>Medicines and medical equipment.</summary>
		VATEX_SA_35,

		/// <summary>Qualifying metals.</summary>
		VATEX_SA_36,

		/// <summary>Private education to citizen.</summary>
		VATEX_SA_EDU,

		/// <summary>Private healthcare to citizen.</summary>
		VATEX_SA_HEA,

		/// <summary>Supply of qualified military goods.</summary>
		VATEX_SA_MLTRY,

		/// <summary>Free text reason provided by the taxpayer on a case-by-case basis.</summary>
		VATEX_SA_OOS
	}
}
