using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Zatca.Enums;
using Zatca.Models;
using Zatca.Xml;

namespace Zatca.Services
{
	/// <summary>
	/// Builds UBL 2.1 XML invoice documents from <see cref="InvoiceData"/> objects.
	/// </summary>
	public class InvoiceXmlBuilder
	{
		/// <summary>
		/// Serializes an <see cref="Invoice"/> object to an <see cref="XmlDocument"/>.
		/// </summary>
		public XmlDocument Serialize(Invoice invoice)
		{
			var serializer = new XmlSerializer(typeof(Invoice));
			var namespaces = new XmlSerializerNamespaces();
			namespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
			namespaces.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
			namespaces.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");

			var xmlDocument = new XmlDocument { PreserveWhitespace = true };

			using (var stream = new MemoryStream())
			{
				using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }))
				{
					serializer.Serialize(writer, invoice, namespaces);
				}

				stream.Position = 0;
				xmlDocument.Load(stream);
			}

			return xmlDocument;
		}

		/// <summary>
		/// Builds a complete UBL XML invoice from the provided <see cref="InvoiceData"/>.
		/// </summary>
		public XmlDocument BuildInvoiceXml(InvoiceData data)
		{
			var invoice = new Invoice
			{
				ID = new ID { Value = data.ID },
				ProfileID = "reporting:1.0",
				IssueDate = data.IssueDate,
				IssueTime = data.IssueTime,
				UUID = data.UUid,
				DocumentCurrencyCode = "SAR",
				TaxCurrencyCode = "SAR",
				InvoiceTypeCode = new InvoiceTypeCode
				{
					name = data.InvoiceTypeCodeName,
					Value = (ushort)data.InvoiceTypeCodeValue
				},
				Delivery = new Delivery { ActualDeliveryDate = data.IssueDate }
			};

			BuildBillingReference(invoice, data);
			BuildAdditionalDocumentReferences(invoice, data);
			BuildSupplierParty(invoice, data);
			BuildCustomerParty(invoice, data);
			BuildPaymentMeans(invoice, data);
			BuildTaxTotals(invoice, data);
			BuildLegalMonetaryTotal(invoice, data);
			invoice.InvoiceLine = BuildInvoiceLines(data.InvoiceLines);

			return Serialize(invoice);
		}

		private static void BuildBillingReference(Invoice invoice, InvoiceData data)
		{
			if (!string.IsNullOrEmpty(data.InvoiceDocumentReference))
			{
				invoice.BillingReference = new BillingReference
				{
					InvoiceDocumentReference = new BillingReferenceInvoiceDocumentReference
					{
						ID = new ID { Value = data.InvoiceDocumentReference }
					}
				};
			}
		}

		private static void BuildAdditionalDocumentReferences(Invoice invoice, InvoiceData data)
		{
			invoice.AdditionalDocumentReference = new[]
			{
				new AdditionalDocumentReference
				{
					ID = new ID { Value = "ICV" },
					UUID = data.ICV
				},
				new AdditionalDocumentReference
				{
					ID = new ID { Value = "PIH" },
					Attachment = new AdditionalDocumentReferenceAttachment
					{
						EmbeddedDocumentBinaryObject = new EmbeddedDocumentBinaryObject
						{
							mimeCode = "text/plain",
							Value = data.PIH
						}
					}
				},
				new AdditionalDocumentReference
				{
					ID = new ID { Value = "QR" },
					Attachment = new AdditionalDocumentReferenceAttachment
					{
						EmbeddedDocumentBinaryObject = new EmbeddedDocumentBinaryObject
						{
							mimeCode = "text/plain",
							Value = data.QR
						}
					}
				}
			};
		}

		private static void BuildSupplierParty(Invoice invoice, InvoiceData data)
		{
			invoice.AccountingSupplierParty = new AccountingSupplierParty
			{
				Party = new AccountingSupplierPartyParty
				{
					PartyIdentification = new AccountingSupplierPartyPartyPartyIdentification
					{
						ID = new ID
						{
							schemeID = data.Supplier.SchemeID.ToString(),
							Value = data.Supplier.SchemaValue
						}
					},
					PostalAddress = new AccountingSupplierPartyPartyPostalAddress
					{
						StreetName = data.Supplier.StreetName,
						BuildingNumber = data.Supplier.BuildingNumber,
						CityName = data.Supplier.CityName,
						PostalZone = data.Supplier.PostalZone,
						PlotIdentification = data.Supplier.PlotIdentification,
						CitySubdivisionName = data.Supplier.CitySubdivisionName,
						Country = new AccountingSupplierPartyPartyPostalAddressCountry
						{
							IdentificationCode = "SA"
						}
					},
					PartyTaxScheme = new AccountingSupplierPartyPartyPartyTaxScheme
					{
						CompanyID = data.Supplier.CompanyID,
						TaxScheme = new AccountingSupplierPartyPartyPartyTaxSchemeTaxScheme
						{
							ID = new ID { Value = "VAT" }
						}
					},
					PartyLegalEntity = new AccountingSupplierPartyPartyPartyLegalEntity
					{
						RegistrationName = data.Supplier.RegistrationName
					}
				}
			};
		}

		private static void BuildCustomerParty(Invoice invoice, InvoiceData data)
		{
			invoice.AccountingCustomerParty = new AccountingCustomerParty
			{
				Party = new AccountingCustomerPartyParty
				{
					PartyIdentification = new AccountingCustomerPartyPartyPartyIdentification
					{
						ID = new ID
						{
							schemeID = data.Customer.SchemeID.ToString(),
							Value = data.Customer.SchemaValue
						}
					},
					PostalAddress = new AccountingCustomerPartyPartyPostalAddress
					{
						StreetName = data.Customer.StreetName,
						BuildingNumber = data.Customer.BuildingNumber,
						CityName = data.Customer.CityName,
						PostalZone = data.Customer.PostalZone,
						PlotIdentification = data.Customer.PlotIdentification,
						CitySubdivisionName = data.Customer.CitySubdivisionName,
						Country = new AccountingCustomerPartyPartyPostalAddressCountry
						{
							IdentificationCode = "SA"
						}
					},
					PartyLegalEntity = new AccountingCustomerPartyPartyPartyLegalEntity
					{
						RegistrationName = data.Customer.RegistrationName
					}
				}
			};
		}

		private static void BuildPaymentMeans(Invoice invoice, InvoiceData data)
		{
			if (data.InstructionNote != null)
			{
				invoice.PaymentMeans = new PaymentMeans
				{
					PaymentMeansCode = 10,
					InstructionNote = data.InstructionNote
				};
			}
		}

		private static void BuildTaxTotals(Invoice invoice, InvoiceData data)
		{
			invoice.TaxTotal = new[]
			{
				new TaxTotal
				{
					TaxAmount = new TaxAmount { Value = data.TaxTotal.TaxAmount, currencyID = "SAR" },
					TaxSubtotal = BuildTaxSubtotals(data.TaxTotal.TaxSubtotal)
				},
				new TaxTotal
				{
					TaxAmount = new TaxAmount { Value = data.TaxTotal.TaxAmount, currencyID = "SAR" }
				}
			};
		}

		private static void BuildLegalMonetaryTotal(Invoice invoice, InvoiceData data)
		{
			var taxableAmount = data.TaxTotal.TaxableAmount;
			var taxAmount = data.TaxTotal.TaxAmount;

			invoice.LegalMonetaryTotal = new LegalMonetaryTotal
			{
				LineExtensionAmount = new LineExtensionAmount { currencyID = "SAR", Value = taxableAmount },
				TaxExclusiveAmount = new TaxExclusiveAmount { currencyID = "SAR", Value = taxableAmount },
				TaxInclusiveAmount = new TaxInclusiveAmount { currencyID = "SAR", Value = taxAmount + taxableAmount },
				AllowanceTotalAmount = new AllowanceTotalAmount { currencyID = "SAR", Value = 0 },
				PayableAmount = new PayableAmount { currencyID = "SAR", Value = taxAmount + taxableAmount }
			};
		}

		private static InvoiceLine[] BuildInvoiceLines(List<InvoiceLineData> lines)
		{
			var invoiceLines = new List<InvoiceLine>();

			foreach (var item in lines)
			{
				invoiceLines.Add(new InvoiceLine
				{
					ID = new ID { Value = item.ID },
					InvoicedQuantity = new InvoicedQuantity { unitCode = "PCE", Value = item.InvoicedQuantity },
					LineExtensionAmount = new LineExtensionAmount { currencyID = "SAR", Value = item.LineExtensionAmount },
					AllowanceCharge = new AllowanceCharge
					{
						ChargeIndicator = false,
						AllowanceChargeReason = "discount",
						Amount = new Amount { Value = item.Discount, currencyID = "SAR" }
					},
					TaxTotal = new InvoiceLineTaxTotal
					{
						TaxAmount = new TaxAmount { Value = item.TaxAmount, currencyID = "SAR" },
						RoundingAmount = new RoundingAmount { currencyID = "SAR", Value = item.RoundingAmount }
					},
					Item = new InvoiceLineItem
					{
						Name = item.Name,
						ClassifiedTaxCategory = new InvoiceLineItemClassifiedTaxCategory
						{
							ID = new ID { Value = item.TaxCategoryId.ToString() },
							Percent = item.VatPercent,
							TaxScheme = new InvoiceLineItemClassifiedTaxCategoryTaxScheme
							{
								ID = new ID { Value = "VAT" }
							}
						}
					},
					Price = new InvoiceLinePrice
					{
						PriceAmount = new PriceAmount { currencyID = "SAR", Value = item.PriceAmount }
					}
				});
			}

			return invoiceLines.ToArray();
		}

		private static TaxTotalTaxSubtotal[] BuildTaxSubtotals(List<InvoiceTaxSubtotal> subtotals)
		{
			var result = new List<TaxTotalTaxSubtotal>();

			foreach (var item in subtotals)
			{
				bool isExemptOrZero = item.TaxCategoryId == TaxCategoryIdType.Z || item.TaxCategoryId == TaxCategoryIdType.E;

				result.Add(new TaxTotalTaxSubtotal
				{
					TaxableAmount = new TaxableAmount { currencyID = "SAR", Value = item.TaxableAmount },
					TaxAmount = new TaxAmount { Value = item.TaxAmount, currencyID = "SAR" },
					TaxCategory = new TaxTotalTaxSubtotalTaxCategory
					{
						ID = new ID { Value = item.TaxCategoryId?.ToString() },
						Percent = item.VatPercent,
						TaxExemptionReason = isExemptOrZero ? item.TaxExemptionReason?.ToString() : null,
						TaxExemptionReasonCode = isExemptOrZero ? item.TaxExemptionReasonCode?.ToString() : null,
						TaxScheme = new TaxTotalTaxSubtotalTaxCategoryTaxScheme
						{
							ID = new ID { Value = "VAT" }
						}
					}
				});
			}

			return result.ToArray();
		}
	}
}
