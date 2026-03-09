using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Zatca.Models
{
	/// <summary>
	/// Represents all the data required to build and submit a ZATCA invoice.
	/// يمثل جميع البيانات المطلوبة لبناء وإرسال فاتورة هيئة الزكاة والضريبة والجمارك.
	/// </summary>
	public class InvoiceData
	{
		public string ID { get; set; }
		public string UUid { get; set; }
		public string IssueDate { get; set; }
		public string IssueTime { get; set; }
		public string BinarySecurityToken { get; set; }
		public string Secret { get; set; }

		/// <summary>
		/// A 7-character string of '0', '1', or '2' indicating the invoice sub-type.
		/// '0200000' for simulation, '0100000' for production.
		/// </summary>
		[RegularExpression("^[012]{7}$", ErrorMessage = "InvoiceTypeCodeName must be a 7-digit string consisting of only '0', '1', or '2' (e.g., '0200000' for simulation, '0100000' for production).")]
		public string InvoiceTypeCodeName { get; set; }

		/// <summary>
		/// The invoice type code value: 388 (standard), 383 (debit note), or 381 (credit note).
		/// </summary>
		public int InvoiceTypeCodeValue { get; set; }

		public string ICV { get; set; }
		public string PIH { get; set; }
		public string InvoiceDocumentReference { get; set; }
		public string InstructionNote { get; set; }
		public string CertificateContent { get; set; }
		public string PrivateKeyContent { get; set; }
		public Uri ZatcaUrl { get; set; }
		public string QR { get; set; }
		public Supplier Supplier { get; set; }
		public Customer Customer { get; set; }
		public InvoiceTaxTotal TaxTotal { get; set; }
		public List<InvoiceLineData> InvoiceLines { get; set; }

		/// <summary>
		/// Creates a new InvoiceData with auto-populated date/time.
		/// </summary>
		public InvoiceData()
		{
			IssueDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
			IssueTime = DateTime.Now.ToString("HH:mm:ss");
		}

		/// <summary>
		/// Creates a new InvoiceData with the specified invoice identification and type settings.
		/// </summary>
		public InvoiceData(
			string id,
			string invoiceTypeCodeName,
			int invoiceTypeCodeValue,
			string pih = "NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==",
			string icv = "46531")
			: this()
		{
			ID = id;
			InvoiceTypeCodeName = invoiceTypeCodeName;
			InvoiceTypeCodeValue = invoiceTypeCodeValue;
			PIH = pih;
			ICV = icv;
		}

		/// <summary>
		/// Creates a fully populated InvoiceData instance.
		/// </summary>
		public InvoiceData(
			string invoiceTypeCodeName,
			int invoiceTypeCodeValue,
			string icv,
			string pih,
			string qr,
			Supplier supplier,
			Customer customer,
			InvoiceTaxTotal taxTotal,
			List<InvoiceLineData> invoiceLines)
			: this()
		{
			ID = "1";
			InvoiceTypeCodeName = invoiceTypeCodeName;
			InvoiceTypeCodeValue = invoiceTypeCodeValue;
			ICV = icv;
			PIH = pih;
			QR = qr;
			Supplier = supplier;
			Customer = customer;
			TaxTotal = taxTotal;
			InvoiceLines = invoiceLines;
		}
	}
}
