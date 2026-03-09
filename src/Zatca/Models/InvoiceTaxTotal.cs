using System;
using System.Collections.Generic;
using Zatca.Enums;

namespace Zatca.Models
{
	/// <summary>
	/// Represents the total tax information for an invoice.
	/// يمثل معلومات الضريبة الإجمالية للفاتورة.
	/// </summary>
	public class InvoiceTaxTotal
	{
		/// <summary>The total tax amount.</summary>
		public decimal TaxAmount { get; }

		/// <summary>The total taxable amount (before tax).</summary>
		public decimal TaxableAmount { get; }

		/// <summary>The total discount amount.</summary>
		public decimal TotalDiscount { get; }

		/// <summary>The overall VAT percentage.</summary>
		public decimal VatPercent { get; }

		/// <summary>The list of tax subtotals broken down by category.</summary>
		public List<InvoiceTaxSubtotal> TaxSubtotal { get; set; }

		public InvoiceTaxTotal(
			decimal taxAmount,
			decimal taxableAmount,
			decimal vatPercent = 0,
			decimal totalDiscount = 0)
		{
			TaxAmount = Math.Round(taxAmount, 2);
			TaxableAmount = Math.Round(taxableAmount, 2);
			TotalDiscount = Math.Round(totalDiscount, 2);
			VatPercent = Math.Round(vatPercent, 2);
		}
	}

	/// <summary>
	/// Represents a tax subtotal entry, typically one per tax category.
	/// يمثل إدخال المجموع الفرعي للضريبة.
	/// </summary>
	public class InvoiceTaxSubtotal
	{
		/// <summary>The tax amount for this subtotal.</summary>
		public decimal TaxAmount { get; }

		/// <summary>The taxable amount for this subtotal.</summary>
		public decimal TaxableAmount { get; }

		/// <summary>The VAT percentage for this subtotal.</summary>
		public decimal VatPercent { get; }

		/// <summary>The tax category (S, Z, or E).</summary>
		public TaxCategoryIdType? TaxCategoryId { get; }

		/// <summary>The tax exemption reason code (required for Z and E categories).</summary>
		public ZatcaTaxExemptionReasonCode? TaxExemptionReasonCode { get; }

		/// <summary>The tax exemption reason (required for Z and E categories).</summary>
		public TaxExemptionReason? TaxExemptionReason { get; }

		public InvoiceTaxSubtotal(
			decimal taxAmount,
			decimal taxableAmount,
			decimal vatPercent = 0,
			TaxCategoryIdType taxCategoryId = TaxCategoryIdType.S,
			ZatcaTaxExemptionReasonCode? taxExemptionReasonCode = null,
			TaxExemptionReason? taxExemptionReason = null)
		{
			TaxableAmount = Math.Round(taxableAmount, 2);
			TaxAmount = Math.Round(taxAmount, 2);
			TaxCategoryId = taxCategoryId;
			TaxExemptionReasonCode = taxExemptionReasonCode;
			TaxExemptionReason = taxExemptionReason;
			VatPercent = Math.Round(vatPercent, 2);
		}
	}
}
