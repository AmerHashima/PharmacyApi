using System.ComponentModel.DataAnnotations;
using Zatca.Enums;

namespace Zatca.Models
{
	/// <summary>
	/// Represents the supplier (seller) party in a ZATCA invoice.
	/// يمثل المورد (البائع) في فاتورة هيئة الزكاة والضريبة والجمارك.
	/// </summary>
	public class Supplier
	{
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// The scheme ID used to identify the supplier.
		/// معرف المخطط المستخدم لتحديد هوية المورد.
		/// </summary>
		[MaxLength(50)]
		public SellerSchemeIDType SchemeID { get; set; }

		/// <summary>
		/// The value associated with the scheme ID (e.g., the actual registration number).
		/// القيمة المرتبطة بمعرف المخطط.
		/// </summary>
		[MaxLength(2000)]
		public string SchemaValue { get; set; }

		[MaxLength(2000)]
		public string StreetName { get; set; }

		[MaxLength(2000)]
		public string BuildingNumber { get; set; }

		[MaxLength(2000)]
		public string PostalZone { get; set; }

		[MaxLength(2000)]
		public string PlotIdentification { get; set; }

		[MaxLength(2000)]
		public string CityName { get; set; }

		/// <summary>
		/// The VAT registration number (Company ID).
		/// الرقم الضريبي (معرف الشركة).
		/// </summary>
		[MaxLength(2000)]
		public string CompanyID { get; set; }

		[MaxLength(2000)]
		public string RegistrationName { get; set; }

		[MaxLength(2000)]
		public string CitySubdivisionName { get; set; }

		public Supplier() { }

		public Supplier(
			SellerSchemeIDType schemeID,
			string schemaValue,
			string streetName,
			string buildingNumber,
			string cityName,
			string companyID,
			string registrationName,
			string postalZone,
			string plotIdentification,
			string citySubdivisionName)
		{
			SchemeID = schemeID;
			SchemaValue = schemaValue;
			StreetName = streetName;
			BuildingNumber = buildingNumber;
			CityName = cityName;
			CompanyID = companyID;
			RegistrationName = registrationName;
			PostalZone = postalZone;
			PlotIdentification = plotIdentification;
			CitySubdivisionName = citySubdivisionName;
		}
	}
}
