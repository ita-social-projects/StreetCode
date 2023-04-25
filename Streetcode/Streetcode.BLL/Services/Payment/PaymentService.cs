using System.Net.Mime;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Streetcode.BLL.Interfaces.Payment;
using Streetcode.BLL.Services.Payment.Exceptions;
using Streetcode.DAL.Entities.Payment;

namespace Streetcode.BLL.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentEnvirovmentVariables _paymentEnvirovment;
        private readonly HttpClient _httpClient;
        public PaymentService(IOptions<PaymentEnvirovmentVariables> paymentEnvirovment)
        {
            _paymentEnvirovment = paymentEnvirovment.Value;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Api.Production);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add(RequestHeaders.XToken, _paymentEnvirovment.Token);
        }

        public async Task<InvoiceInfo> CreateInvoiceAsync(Invoice invoice)
        {
            var (code, body) = await PostAsync(Api.Merchant.Invoice.Create, invoice);
            return code switch
            {
                200 => JsonToObject<InvoiceInfo>(body),
                400 => throw new InvalidRequestParameterException(JsonToObject<Error>(body)),
                403 => throw new InvalidTokenException(),
                _ => throw new NotSupportedException()
            };
        }

        private async Task<(int Code, string Body)> PostAsync<T>(string url, T data)
        {
                var jsonString = JsonConvert.SerializeObject(data, Formatting.None);
                var content = new StringContent(jsonString, Encoding.UTF8, MediaTypeNames.Application.Json);
                var response = await _httpClient.PostAsync(url, content);
                return (
                    Code: (int)response.StatusCode,
                    Body: await response.Content.ReadAsStringAsync());
        }

        private T JsonToObject<T>(string body)
        {
            return JsonConvert.DeserializeObject<T>(body);
        }

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

        private static class RequestHeaders
        {
            public const string XToken = "X-Token";
        }

        private static class Validation
        {
            public const int MaxStatementTimeSpanInSeconds = 2682000;
            public const int StatementTimeoutBetweenCallsInSeconds = 60;
        }
    }
}
