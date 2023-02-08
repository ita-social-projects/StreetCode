
using System.Text.Json;


namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    public class CaseIsensitiveJsonDeserializer
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static T Deserialize<T>(string text)
        {
            return JsonSerializer.Deserialize<T>(text, options);
        }
    }

}
