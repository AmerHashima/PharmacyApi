using System.ComponentModel.DataAnnotations;
using Zatca.Enums;

namespace Zatca.Models
{
	/// <summary>
	/// Represents the customer (buyer) party in a ZATCA invoice.
	/// يمثل العميل (المشتري) في فاتورة هيئة الزكاة والضريبة والجمارك.
	/// </summary>
	public class Customer
	{
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// The scheme ID used to identify the customer.
		/// معرف المخطط المستخدم لتحديد هوية العميل.
		/// </summary>
		[Required]
		public CustomerSchemeIDType SchemeID { get; set; }

		[MaxLength(50)]
		public string SchemaValue { get; set; }

		[MaxLength(100)]
		public string StreetName { get; set; }

		[MaxLength(50)]
		public string BuildingNumber { get; set; }

		[MaxLength(20)]
		public string PostalZone { get; set; }

		[MaxLength(50)]
		public string PlotIdentification { get; set; }

		[MaxLength(50)]
		public string CityName { get; set; }

		[MaxLength(50)]
		public string CompanyID { get; set; }

		[MaxLength(100)]
		public string RegistrationName { get; set; }

		[MaxLength(50)]
		public string CitySubdivisionName { get; set; }

		public Customer() { }

		public Customer(
			CustomerSchemeIDType schemeID,
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
