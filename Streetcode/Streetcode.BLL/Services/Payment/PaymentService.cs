using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using Streetcode.BLL.Interfaces.Payment;
using Streetcode.DAL.Entities.Payment;

namespace Streetcode.BLL.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        public PaymentService(string Token)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Api.Production);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add(RequestHeaders.XToken, Token);
        }

        public async Task<InvoiceInfo> CreateInvoiceAsync(Invoice invoice)
        {
            var paymentService = new PaymentService("uyV-54sYFBWRUmgHWdNBW6qmz8V35Nzy1rY-Hc3EQ5sY");
            var result = await paymentService.CreateInvoiceService(new Invoice
            {
                Amount = 100
            });
            return result;
        }

        public async Task<InvoiceInfo> CreateInvoiceService(Invoice invoice)
        {
            var (code, body) = await PostAsync(Api.Merchant.Invoice.Create, invoice);
            return code switch
            {
                200 => JsonConvert.DeserializeObject<InvoiceInfo>(body)
            };
        }

        /// <summary>
        /// POSTs data to the given <see cref="url"/> of Monobank's API.
        /// </summary>
        /// <param name="url">the URL path to which the request will be made.</param>
        /// <param name="data">the data which will be POSTed.</param>
        /// <exception cref="MonobankRequestException">Thrown if request to Monobank's API failed.</exception>
        /// <returns>Returns the tuple of HTTP Status Code and JSON Body received in response from Monobank's API.</returns>
        private async Task<(int Code, string Body)> PostAsync<T>(string url, T data)
        {
                var jsonString = JsonConvert.SerializeObject(data, Formatting.None);
                var content = new StringContent(jsonString, Encoding.UTF8, MediaTypeNames.Application.Json);
                var response = await _httpClient.PostAsync(url, content);
                return (
                    Code: (int)response.StatusCode,
                    Body: await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Encapsulates constants related to API requests, URLs and URL parts.
        /// </summary>
        private static class Api
        {
            public const string Production = "https://api.monobank.ua";

            public static class Merchant
            {
                public static class Invoice
                {
                    public const string Create = "/api/merchant/invoice/create";
                }
            }
        }

        /// <summary>
        /// Encapsulates constants related to HTTP Headers used when making calls to Monobank's API.
        /// </summary>
        private static class RequestHeaders
        {
            public const string XToken = "uyV-54sYFBWRUmgHWdNBW6qmz8V35Nzy1rY-Hc3EQ5sY";
        }

        /// <summary>
        /// Encapsulates constants used by validation methods and checks.
        /// </summary>
        private static class Validation
        {
            public const int MaxStatementTimeSpanInSeconds = 2682000;
            public const int StatementTimeoutBetweenCallsInSeconds = 60;
        }
    }
}
