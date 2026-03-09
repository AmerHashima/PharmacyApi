using System;
using System.IO;
using System.Xml;
using ZATCA.EInvoice.SDK;
using ZATCA.EInvoice.SDK.Contracts.Models;
using Zatca.Models;

namespace Zatca.Services
{
	/// <summary>
	/// Modifies XML invoice files in bulk (e.g., updating CompanyID/UUID),
	/// signs them, and submits to the ZATCA compliance endpoint.
	/// Used during the initial CSR onboarding process.
	/// </summary>
	public class XmlModificationService
	{
		private readonly CsrService _csrService;

		public XmlModificationService(CsrService csrService)
		{
			_csrService = csrService;
		}

		/// <summary>
		/// Processes all XML invoice files in the specified folder: updates the target element value,
		/// signs each invoice, and submits it for compliance validation.
		/// </summary>
		public void ProcessComplianceInvoices(
			string folderPath,
			string targetElement,
			string newValue,
			CertificateResponse certificateResponse,
			CsrSetting csrSetting,
			CsrResponse csrResponse)
		{
			string guidValue = Guid.NewGuid().ToString();
			string[] xmlFiles = Directory.GetFiles(folderPath, "*.xml");

			foreach (string filePath in xmlFiles)
			{
				try
				{
					if (targetElement == "UUID")
						guidValue = newValue;

					string namespaceUri = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";

					var doc = new XmlDocument { PreserveWhitespace = true };
					doc.Load(filePath);

					var nsManager = new XmlNamespaceManager(doc.NameTable);
					nsManager.AddNamespace("cbc", namespaceUri);

					XmlNode targetNode = doc.SelectSingleNode($"//cbc:{targetElement}", nsManager);
					XmlNode uuidNode = doc.SelectSingleNode("//cbc:UUID", nsManager);

					if (targetNode != null)
					{
						targetNode.InnerText = newValue;
						if (uuidNode != null)
							uuidNode.InnerText = guidValue;
					}

					SignAndSubmitForCompliance(doc, certificateResponse, csrSetting, csrResponse);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error processing XML file: {filePath}\n{ex.Message}");
				}
			}
		}

		private void SignAndSubmitForCompliance(
			XmlDocument xmlDoc,
			CertificateResponse certificateResponse,
			CsrSetting csrSetting,
			CsrResponse csrResponse)
		{
			var signer = new EInvoiceSigner();
			xmlDoc.PreserveWhitespace = true;

			SignResult signResult = signer.SignDocument(xmlDoc, certificateResponse.CertificateContent, certificateResponse.PrivateKeyContent);

			var requestGenerator = new RequestGenerator();
			RequestResult requestResult = requestGenerator.GenerateRequest(signResult.SignedEInvoice);

			var hashGenerator = new EInvoiceHashGenerator();
			HashResult hashResult = hashGenerator.GenerateEInvoiceHashing(signResult.SignedEInvoice);
			requestResult.InvoiceRequest.InvoiceHash = hashResult.Hash;

			_csrService.SubmitComplianceInvoice(csrResponse, csrSetting, requestResult.InvoiceRequest);
		}
	}
}
