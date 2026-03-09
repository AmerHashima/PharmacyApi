using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using ZATCA.EInvoice.SDK;
using ZATCA.EInvoice.SDK.Contracts.Models;
using Zatca.Models;

namespace Zatca.Services
{
	/// <summary>
	/// Handles CSR generation, compliance certificate requests, and production CSID issuance
	/// with the ZATCA portal.
	/// </summary>
	public class CsrService
	{
		/// <summary>
		/// Executes the full CSR creation and production CSID workflow:
		/// 1. Generate local CSR and private key
		/// 2. Request compliance CSID
		/// 3. Submit compliance invoices
		/// 4. Request production CSID
		/// </summary>
		public CertificateResponse CreateCsrAndObtainProductionCsid(CsrSetting csrSetting)
		{
			var certificateResponse = new CertificateResponse();

			CsrResponse complianceResponse = GenerateCsrAndRequestComplianceCsid(csrSetting, certificateResponse);

			byte[] decodedBytes = Convert.FromBase64String(complianceResponse.BinarySecurityToken);
			certificateResponse.CertificateContent = Encoding.UTF8.GetString(decodedBytes);

			var xmlService = new XmlModificationService(this);
			xmlService.ProcessComplianceInvoices(
				"C:\\InvoiceSetup\\zatcainvoices",
				"CompanyID",
				csrSetting.VatRegNo,
				certificateResponse,
				csrSetting,
				complianceResponse);

			CsrResponse productionResponse = RequestProductionCsid(csrSetting, complianceResponse);

			certificateResponse.BinarySecurityToken = productionResponse.BinarySecurityToken;
			certificateResponse.Secret = productionResponse.Secret;
			decodedBytes = Convert.FromBase64String(productionResponse.BinarySecurityToken);
			certificateResponse.CertificateContent = Encoding.UTF8.GetString(decodedBytes);

			return certificateResponse;
		}

		/// <summary>
		/// Generates a local CSR and private key, then requests a compliance CSID from ZATCA.
		/// </summary>
		public CsrResponse GenerateCsrAndRequestComplianceCsid(CsrSetting csrSetting, CertificateResponse certificateResponse)
		{
			var providerSetting = new ProviderSetting(
				developProviderName: "ExampleProvider",
				developVersionNumber: "1.0.0",
				developGUID: "123e4567-e89b-12d3-a456-426614174000");

			var csrGenerationDto = new CsrGenerationDto(
				csrSetting.NameEn.Replace(" ", ""),
				"1-" + providerSetting.DevelopProviderName + "|2-" + providerSetting.DevelopVersionNumber + "|3-" + providerSetting.DevelopGUID,
				csrSetting.VatRegNo,
				csrSetting.UnitNameEn.Replace(" ", ""),
				csrSetting.NameEn.Replace(" ", ""),
				"SA",
				"1100",
				csrSetting.Address,
				csrSetting.Category);

			var csrGenerator = new CsrGenerator();
			var environmentType = csrSetting.InvoicePortalType == "2" ? EnvironmentType.Simulation : EnvironmentType.Production;
			CsrResult csrResult = csrGenerator.GenerateCsr(csrGenerationDto, environmentType);

			certificateResponse.CertificateContent = csrResult.Csr;
			certificateResponse.PrivateKeyContent = csrResult.PrivateKey;

			CsrResponse complianceResponse = RequestComplianceCsid(csrSetting, csrResult.Csr);
			if (complianceResponse == null)
			{
				throw new InvalidOperationException("Failed to obtain compliance CSID from ZATCA portal.");
			}

			return complianceResponse;
		}

		/// <summary>
		/// Sends a CSR to the ZATCA portal to obtain a compliance CSID.
		/// </summary>
		public CsrResponse RequestComplianceCsid(CsrSetting csrSetting, string localCsr)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(csrSetting.ZatcaLink);
				client.DefaultRequestHeaders.Add("OTP", csrSetting.Otp);
				client.DefaultRequestHeaders.Add("Accept-Version", "V2");

				var requestBody = new { csr = localCsr };
				var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

				var result = client.PostAsync("compliance", content).Result;
				if (result.StatusCode != System.Net.HttpStatusCode.OK)
				{
					var errorResponse = result.Content.ReadAsStringAsync().Result;
					throw new HttpRequestException($"Compliance CSID request failed: {errorResponse}");
				}

				return JsonConvert.DeserializeObject<CsrResponse>(result.Content.ReadAsStringAsync().Result);
			}
		}

		/// <summary>
		/// Submits a signed invoice to the ZATCA compliance endpoint for validation.
		/// </summary>
		public void SubmitComplianceInvoice(CsrResponse csrResponse, CsrSetting csrSetting, InvoiceRequest invoiceRequest)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(csrSetting.ZatcaLink);
				client.DefaultRequestHeaders.Add("Accept-Version", "V2");
				client.DefaultRequestHeaders.Add("accept-language", "en");

				string credentials = Convert.ToBase64String(
					Encoding.GetEncoding("ISO-8859-1").GetBytes(csrResponse.BinarySecurityToken + ":" + csrResponse.Secret));
				client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);

				string serialized = invoiceRequest.Serialize();
				var content = new StringContent(serialized, Encoding.UTF8, "application/json");
				var result = client.PostAsync("compliance/invoices", content).Result;
				// Response consumed for validation; errors handled by caller if needed
				result.EnsureSuccessStatusCode();
			}
		}

		/// <summary>
		/// Requests a production CSID from ZATCA using the compliance CSID credentials.
		/// </summary>
		public CsrResponse RequestProductionCsid(CsrSetting csrSetting, CsrResponse complianceResponse)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(csrSetting.ZatcaLink);
				client.DefaultRequestHeaders.Add("Accept-Version", "V2");

				string credentials = Convert.ToBase64String(
					Encoding.GetEncoding("ISO-8859-1").GetBytes(complianceResponse.BinarySecurityToken + ":" + complianceResponse.Secret));
				client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);

				var requestBody = new { compliance_request_id = complianceResponse.RequestID };
				var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

				var result = client.PostAsync("production/csids", content).Result;
				return JsonConvert.DeserializeObject<CsrResponse>(result.Content.ReadAsStringAsync().Result);
			}
		}
	}
}
