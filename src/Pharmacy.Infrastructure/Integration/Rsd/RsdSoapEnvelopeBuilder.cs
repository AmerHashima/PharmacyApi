using System.Text;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Infrastructure.Integration.Rsd;

public static class RsdSoapEnvelopeBuilder
{
    public static string BuildAcceptDispatchEnvelope(string dispatchNotificationId)
    {
        return $"""
            <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:dis="http://dtts.sfda.gov.sa/AcceptDispatchService">
               <soapenv:Header/>
               <soapenv:Body>
                  <dis:AcceptDispatchServiceRequest>
                     <DISPATCHNOTIFICATIONID>{EscapeXml(dispatchNotificationId)}</DISPATCHNOTIFICATIONID>
                  </dis:AcceptDispatchServiceRequest>
               </soapenv:Body>
            </soapenv:Envelope>
            """;
    }

    public static string BuildDispatchDetailEnvelope(string dispatchNotificationId)
    {
        return $"""
            <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:dis="http://dtts.sfda.gov.sa/DispatchDetailService">
               <soapenv:Header/>
               <soapenv:Body>
                  <dis:DispatchDetailServiceRequest>
                     <DISPATCHNOTIFICATIONID>{EscapeXml(dispatchNotificationId)}</DISPATCHNOTIFICATIONID>
                  </dis:DispatchDetailServiceRequest>
               </soapenv:Body>
            </soapenv:Envelope>
            """;
    }

    public static string BuildAcceptBatchEnvelope(string fromGln, List<AcceptBatchProductItemDto> products)
    {
        var productListXml = new StringBuilder();
        foreach (var product in products)
        {
            productListXml.AppendLine($"""
                        <PRODUCT>
                           <GTIN>{EscapeXml(product.GTIN)}</GTIN>
                           <QUANTITY>{product.Quantity}</QUANTITY>
                           <BN>{EscapeXml(product.BatchNumber)}</BN>
                           <XD>{EscapeXml(product.ExpiryDate)}</XD>
                        </PRODUCT>
            """);
        }

        return $"""
            <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:acc="http://dtts.sfda.gov.sa/AcceptBatchService">
               <soapenv:Header/>
               <soapenv:Body>
                  <acc:AcceptBatchServiceRequest>
                     <FROMGLN>{EscapeXml(fromGln)}</FROMGLN>
                     <PRODUCTLIST>
            {productListXml}
                     </PRODUCTLIST>
                  </acc:AcceptBatchServiceRequest>
               </soapenv:Body>
            </soapenv:Envelope>
            """;
    }

    public static string BuildPharmacySaleEnvelope(string fromGln, List<PharmacySaleProductItemDto> products)
    {
        var productListXml = new StringBuilder();
        foreach (var product in products)
        {
            productListXml.AppendLine($"""
                        <PRODUCT>
                           <GTIN>{EscapeXml(product.GTIN)}</GTIN>
                           <XD>{EscapeXml(product.ExpiryDate)}</XD>
                           <BN>{EscapeXml(product.BatchNumber)}</BN>
                           <SN>{EscapeXml(product.SerialNumber)}</SN>
                        </PRODUCT>
            """);
        }

        return $"""
            <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:acc="http://dtts.sfda.gov.sa/AcceptService">
               <soapenv:Header/>
               <soapenv:Body>
                  <acc:AcceptServiceRequest>
                     <FROMGLN>{EscapeXml(fromGln)}</FROMGLN>
                     <PRODUCTLIST>
            {productListXml}
                     </PRODUCTLIST>
                  </acc:AcceptServiceRequest>
               </soapenv:Body>
            </soapenv:Envelope>
            """;
    }

    public static string BuildPharmacySaleCancelEnvelope(string fromGln, List<PharmacySaleCancelProductItemDto> products)
    {
        var productListXml = new StringBuilder();
        foreach (var product in products)
        {
            productListXml.AppendLine($"""
                        <PRODUCT>
                           <GTIN>{EscapeXml(product.GTIN)}</GTIN>
                           <QUANTITY>{product.Quantity}</QUANTITY>
                           <BN>{EscapeXml(product.BatchNumber)}</BN>
                           <XD>{EscapeXml(product.ExpiryDate)}</XD>
                        </PRODUCT>
            """);
        }

        return $"""
            <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:acc="http://dtts.sfda.gov.sa/AcceptBatchService">
               <soapenv:Header/>
               <soapenv:Body>
                  <acc:AcceptBatchServiceRequest>
                     <FROMGLN>{EscapeXml(fromGln)}</FROMGLN>
                     <PRODUCTLIST>
            {productListXml}
                     </PRODUCTLIST>
                  </acc:AcceptBatchServiceRequest>
               </soapenv:Body>
            </soapenv:Envelope>
            """;
    }

    private static string EscapeXml(string value)
    {
        return value
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&apos;");
    }
}