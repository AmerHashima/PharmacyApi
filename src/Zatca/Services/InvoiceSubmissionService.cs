using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Xml;
using ZATCA.EInvoice.SDK;
using ZATCA.EInvoice.SDK.Contracts.Models;
using Zatca.Models;

namespace Zatca.Services
{
	/// <summary>
	/// Handles signing, hashing, and submitting invoices to the ZATCA portal
	/// (both reporting for simplified invoices and clearance for standard invoices).
	/// </summary>
	public class InvoiceSubmissionService
	{
		private readonly InvoiceXmlBuilder _xmlBuilder = new InvoiceXmlBuilder();

		/// <summary>
		/// Builds, signs, and submits an invoice to the ZATCA portal.
		/// Returns an anonymous object with the response, PIH, and error messages.
		/// </summary>
		public dynamic SubmitInvoice(InvoiceData invoiceData)
		{
			XmlDocument xmlDocument = _xmlBuilder.BuildInvoiceXml(invoiceData);
			InvoiceRequest invoiceRequest = SignAndPrepareRequest(xmlDocument, invoiceData);

			if (invoiceData.InvoiceTypeCodeName == "0200000")
				return ReportInvoice(invoiceData, invoiceRequest);
			else
				return ClearInvoice(invoiceData, invoiceRequest);
		}

		private static InvoiceRequest SignAndPrepareRequest(XmlDocument xmlDocument, InvoiceData invoiceData)
		{
			var signer = new EInvoiceSigner();
			xmlDocument.PreserveWhitespace = true;

			SignResult signResult = signer.SignDocument(xmlDocument, invoiceData.CertificateContent, invoiceData.PrivateKeyContent);
			signResult.SaveSignedEInvoice("C:\\InvoiceSetup\\" + invoiceData.ID + ".xml");

			var requestGenerator = new RequestGenerator();
			RequestResult requestResult = requestGenerator.GenerateRequest(signResult.SignedEInvoice);

			var hashGenerator = new EInvoiceHashGenerator();
			HashResult hashResult = hashGenerator.GenerateEInvoiceHashing(signResult.SignedEInvoice);
			requestResult.InvoiceRequest.InvoiceHash = hashResult.Hash;

			return requestResult.InvoiceRequest;
		}

		private static dynamic ReportInvoice(InvoiceData invoiceData, InvoiceRequest invoiceRequest)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = invoiceData.ZatcaUrl;
				ConfigureHeaders(client, invoiceData.BinarySecurityToken, invoiceData.Secret);

				string serialized = invoiceRequest.Serialize();
				var content = new StringContent(serialized, Encoding.UTF8, "application/json");
				var response = client.PostAsync("invoices/reporting/single", content).Result;
				var responseBody = response.Content.ReadAsStringAsync().Result;

				var validationResponse = JsonConvert.DeserializeObject<ZatcaValidationResponse>(responseBody);
				string errorMsg = ExtractErrorMessages(validationResponse);

				return new { clearanceResponse = validationResponse, Pih = invoiceRequest.InvoiceHash, ErrorMsg = errorMsg };
			}
		}

		private static dynamic ClearInvoice(InvoiceData invoiceData, InvoiceRequest invoiceRequest)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = invoiceData.ZatcaUrl;
				ConfigureHeaders(client, invoiceData.BinarySecurityToken, invoiceData.Secret);

				string serialized = invoiceRequest.Serialize();
				var content = new StringContent(serialized, Encoding.UTF8, "application/json");
				var response = client.PostAsync("invoices/clearance/single", content).Result;
				var responseBody = response.Content.ReadAsStringAsync().Result;

				var validationResponse = JsonConvert.DeserializeObject<ZatcaValidationResponse>(responseBody);
				string errorMsg = ExtractErrorMessages(validationResponse);

				return new { clearanceResponse = validationResponse, Pih = invoiceRequest.InvoiceHash, ErrorMsg = errorMsg };
			}
		}

		private static void ConfigureHeaders(HttpClient client, string binarySecurityToken, string secret)
		{
			client.DefaultRequestHeaders.Add("Accept-Version", "V2");
			client.DefaultRequestHeaders.Add("accept", "application/json");
			client.DefaultRequestHeaders.Add("accept-language", "en");

			string credentials = Convert.ToBase64String(
				Encoding.GetEncoding("ISO-8859-1").GetBytes(binarySecurityToken + ":" + secret));
			client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);
		}

		private static string ExtractErrorMessages(ZatcaValidationResponse response)
		{
			var errorMsg = "";
			if (response?.ValidationResults?.ErrorMessages != null)
			{
				foreach (var error in response.ValidationResults.ErrorMessages)
				{
					errorMsg += error.Message;
				}
			}
			return errorMsg;
		}
	}
}
