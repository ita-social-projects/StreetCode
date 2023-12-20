using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.DAL.Entities.Timeline;
using Newtonsoft.Json;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode.Create
{
    public class StreetcodeCreateControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public StreetcodeCreateControllerTests(CustomWebApplicationFactory<Program> factory)
           : base(factory, "/api/Streetcode")
        {

        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_ReturnsSuccessStatusCode()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;

            // Act
            var response = await client.CreateAsync(streetcodeCreateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_CreatesNewStreetcode()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;

            // Act
            var response = await client.CreateAsync(streetcodeCreateDTO);
            var streetcodeId = StreetcodeIndexFetch.GetStreetcodeByIndex(streetcodeCreateDTO.Index);
            var getResponse = await client.GetByIdAsync(streetcodeId);
            var fetchedStreetcode = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeContent>(getResponse.Content);

            // Assert
            // Check if the fetched Streetcode matches the one we created
            Assert.Equal(streetcodeCreateDTO.Title, fetchedStreetcode.Title);
            Assert.Equal(streetcodeCreateDTO.TransliterationUrl, fetchedStreetcode.TransliterationUrl);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;
            streetcodeCreateDTO.Title = null;  // Invalid data

            // Act
            var response = await client.CreateAsync(streetcodeCreateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractTestStreetcode]
        [ExtractCreateTestStreetcode]
        public async Task Create_WithExistingStreetcode_ReturnsConflict()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;
            streetcodeCreateDTO.Index = ExtractTestStreetcode.StreetcodeForTest.Index;

            // Act
            var response = await client.CreateAsync(streetcodeCreateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_WithLongTransliterationUrl_ReturnsBadRequest()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;
            streetcodeCreateDTO.TransliterationUrl = new string('a', 151);  // TransliterationUrl length exceeds the maximum limit

            // Act
            var response = await client.CreateAsync(streetcodeCreateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}