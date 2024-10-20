namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    using System.Text.Json;

    public static class CaseIsensitiveJsonDeserializer
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public static T? Deserialize<T>(string? text)
            where T : class
        {
            if (text == null)
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(text, options);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
