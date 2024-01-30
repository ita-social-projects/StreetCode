using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter
{
    public static class TestDataProvider
    {
        public static T GetTestData<T>()
        {
            string jsonFilePath = @"../../../testData.json";
            using StreamReader reader = new (jsonFilePath);
            string fileJson = reader.ReadToEnd();
            var patsedFileJson = JObject.Parse(fileJson);

            string entityJson = patsedFileJson[nameof(T)]?.ToString();
            T entity = JsonConvert.DeserializeObject<T>(entityJson);
            return entity;
        }
    }
}
