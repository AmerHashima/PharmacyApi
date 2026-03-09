namespace Zatca.Enums
{
	/// <summary>
	/// Tax exemption reason categories for ZATCA invoicing.
	/// </summary>
	public enum TaxExemptionReason
	{
		/// <summary>Financial services mentioned in Article 29 of the VAT Regulations.</summary>
		FinancialServicesArticle29 = 1,

		/// <summary>Life insurance services mentioned in Article 29 of the VAT Regulations.</summary>
		LifeInsuranceServicesArticle29,

		/// <summary>Real estate transactions mentioned in Article 30 of the VAT Regulations.</summary>
		RealEstateTransactionsArticle30,

		/// <summary>Export of goods.</summary>
		ExportOfGoods,

		/// <summary>Export of services.</summary>
		ExportOfServices,

		/// <summary>The international transport of goods.</summary>
		InternationalTransportOfGoods,

		/// <summary>International transport of passengers.</summary>
		InternationalTransportOfPassengers,

		/// <summary>Services directly connected and incidental to a supply of international passenger transport.</summary>
		ServicesConnectedToIntlPassengerTransport,

		/// <summary>Supply of a qualifying means of transport.</summary>
		SupplyOfQualifyingMeansOfTransport,

		/// <summary>Any services relating to goods or passenger transportation, as defined in Article 25.</summary>
		ServicesRelatingToGoodsOrTransport,

		/// <summary>Medicines and medical equipment.</summary>
		MedicinesAndMedicalEquipment,

		/// <summary>Qualifying metals.</summary>
		QualifyingMetals,

		/// <summary>Private education to citizen.</summary>
		PrivateEducationToCitizen,

		/// <summary>Private healthcare to citizen.</summary>
		PrivateHealthcareToCitizen,

		/// <summary>Supply of qualified military goods.</summary>
		SupplyOfQualifiedMilitaryGoods,

		/// <summary>Free text reason provided by the taxpayer on a case-by-case basis.</summary>
		FreeTextReason
	}

	/// <summary>
	/// Provides human-readable descriptions for each <see cref="TaxExemptionReason"/> value.
	/// </summary>
	public static class TaxExemptionReasonDescriptions
	{
		public const string FinancialServicesArticle29 = "Financial services mentioned in Article 29 of the VAT Regulations.";
		public const string LifeInsuranceServicesArticle29 = "Life insurance services mentioned in Article 29 of the VAT Regulations.";
		public const string RealEstateTransactionsArticle30 = "Real estate transactions mentioned in Article 30 of the VAT Regulations.";
		public const string ExportOfGoods = "Export of goods.";
		public const string ExportOfServices = "Export of services.";
		public const string InternationalTransportOfGoods = "The international transport of Goods.";
		public const string InternationalTransportOfPassengers = "International transport of passengers.";
		public const string ServicesConnectedToIntlPassengerTransport = "Services directly connected and incidental to a Supply of international passenger transport.";
		public const string SupplyOfQualifyingMeansOfTransport = "Supply of a qualifying means of transport.";
		public const string ServicesRelatingToGoodsOrTransport = "Any services relating to Goods or passenger transportation, as defined in Article 25 of these Regulations.";
		public const string MedicinesAndMedicalEquipment = "Medicines and medical equipment.";
		public const string QualifyingMetals = "Qualifying metals.";
		public const string PrivateEducationToCitizen = "Private education to citizen.";
		public const string PrivateHealthcareToCitizen = "Private healthcare to citizen.";
		public const string SupplyOfQualifiedMilitaryGoods = "Supply of qualified military goods.";
		public const string FreeTextReason = "The reason is a free text, provided by the taxpayer on a case-to-case basis.";
	}
}
