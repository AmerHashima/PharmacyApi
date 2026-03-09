using System;
using Zatca.Enums;

namespace Zatca.Models
{
	/// <summary>
	/// Represents a single line item within a ZATCA invoice.
	/// يمثل عنصر واحد في خط الفاتورة.
	/// </summary>
	public class InvoiceLineData
	{
		/// <summary>The unique identifier for this invoice line.</summary>
		public string ID { get; }

		/// <summary>The quantity of items invoiced.</summary>
		public decimal InvoicedQuantity { get; }

		/// <summary>The total amount for this line after discount (Qty × Price − Discount).</summary>
		public decimal LineExtensionAmount { get; }

		/// <summary>The tax amount calculated for this line.</summary>
		public decimal TaxAmount { get; }

		/// <summary>The total rounding amount (LineExtensionAmount + TaxAmount).</summary>
		public decimal RoundingAmount { get; }

		/// <summary>The name of the item.</summary>
		public string Name { get; }

		/// <summary>The unit price of the item.</summary>
		public decimal PriceAmount { get; }

		/// <summary>The VAT percentage applied to this item.</summary>
		public decimal VatPercent { get; }

		/// <summary>The discount amount for this line.</summary>
		public decimal Discount { get; }

		/// <summary>The tax category for this item (S, Z, or E).</summary>
		public TaxCategoryIdType TaxCategoryId { get; }

		public InvoiceLineData(
			string id,
			decimal invoicedQuantity,
			string name,
			decimal priceAmount,
			decimal vatPercent,
			TaxCategoryIdType taxCategoryId,
			decimal discount)
		{
			ID = id;
			Name = name;
			PriceAmount = Math.Round(priceAmount, 2);
			VatPercent = Math.Round(vatPercent, 2);
			TaxCategoryId = taxCategoryId;
			Discount = Math.Round(discount, 2);
			InvoicedQuantity = invoicedQuantity;
			LineExtensionAmount = Math.Round((invoicedQuantity * PriceAmount) - Discount, 2);
			TaxAmount = Math.Round(LineExtensionAmount * (vatPercent / 100), 2);
			RoundingAmount = Math.Round(LineExtensionAmount + TaxAmount, 2);
		}
	}
}
