using Newtonsoft.Json;

namespace Streetcode.BLL.Services.Payment.Exceptions
{
    internal class Error
    {
        [JsonConstructor]
        public Error(string errCode, string errText)
        {
            Code = errCode;
            Text = errText;
        }

        [JsonProperty("errCode")]
        public string Code { get; }

        [JsonProperty("errText")]
        public string Text { get; }
    }
}
