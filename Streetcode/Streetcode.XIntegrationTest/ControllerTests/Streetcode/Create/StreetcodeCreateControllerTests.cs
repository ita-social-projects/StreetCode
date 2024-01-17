﻿using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using System.Net;
using Xunit;

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
            var transliterationUrlMaxLength = 150;
            var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;
            streetcodeCreateDTO.TransliterationUrl = new string('a', transliterationUrlMaxLength + 1);

            // Act
            var response = await client.CreateAsync(streetcodeCreateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}