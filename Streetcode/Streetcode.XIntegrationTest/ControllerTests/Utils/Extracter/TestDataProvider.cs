using Newtonsoft.Json;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter
{
    public static class TestDataProvider
    {
        public static T GetTestData<T>()
        {
            string typeName = typeof(T).Name;
            string jsonFilePath = @$"../../../TestData/{typeName}.json";
            using StreamReader reader = new (jsonFilePath);
            string fileJson = reader.ReadToEnd();
            T entity = JsonConvert.DeserializeObject<T>(fileJson) !;
            return entity;
        }
    }
}
