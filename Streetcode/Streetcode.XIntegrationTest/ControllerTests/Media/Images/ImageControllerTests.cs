﻿using System.Net;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Media.Images
{
    [Collection("Authorization")]
    public class ImageControllerTests : BaseAuthorizationControllerTests<ImageClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Image testImage;
        private readonly StreetcodeContent testStreetcodeContent;

        public ImageControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
            : base(factory, "/api/Image", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testImage = ImageExtracter.Extract(uniqueId);
            this.testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    uniqueId,
                    uniqueId,
                    Guid.NewGuid().ToString());
        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            Image expectedImage = this.testImage;
            var response = await this.Client.GetByIdAsync(expectedImage.Id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<ImageDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Multiple(
                () => Assert.Equal(expectedImage.Id, returnedValue.Id),
                () => Assert.Equal(expectedImage.BlobName, returnedValue.BlobName),
                () => Assert.Equal(expectedImage.ImageDetails?.Id, returnedValue.ImageDetails?.Id));
        }

        [Fact]
        public async Task GetById_Incorrect_ReturnBadRequest()
        {
            int id = -100;
            var response = await this.Client.GetByIdAsync(id);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            ImageExtracter.AddStreetcodeImage(this.testStreetcodeContent.Id, this.testImage.Id);
            int streetcodeId = this.testStreetcodeContent.Id;
            var response = await this.Client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<ImageDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetByStreetcodeId_Incorrect_ReturnBadRequest()
        {
            int streetcodeId = -100;
            var response = await this.Client.GetByStreetcodeId(streetcodeId);

            Assert.Multiple(
              () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
              () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetBaseImage_ReturnSuccessStatusCode()
        {
            Image expectedImage = this.testImage;

            var response = await this.Client.GetBaseImageAsync(expectedImage.Id);

            byte[] returnedValue = response.RawBytes;
            string base64 = Convert.ToBase64String(returnedValue);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            Assert.Equal(expectedImage.Base64, base64);
        }

        [Fact]
        public async Task GetBaseImage_Incorrect_ReturnBadRequest()
        {
            int streetcodeId = -100;

            var response = await this.Client.GetBaseImageAsync(streetcodeId);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        [ExtractCreateTestImage]
        public async Task Create_ReturnSuccessStatusCode()
        {
            var imageCreateDto = ExtractCreateTestImageAttribute.ImageFileCreateForTest;

            var response = await this.Client.CreateAsync(imageCreateDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("-1")]
        [InlineData("3")]
        [ExtractCreateTestImage]
        public async Task Create_IncorrectAlt_ReturnBadRequest(string incorrectAlt)
        {
            var imageCreateDto = ExtractCreateTestImageAttribute.ImageFileCreateForTest;
            imageCreateDto.Alt = incorrectAlt;

            var response = await this.Client.CreateAsync(imageCreateDto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestImage]
        public async Task Update_ReturnSuccessStatusCode()
        {
            var imageUpdateDto = ExtractCreateTestImageAttribute.ImageFileUpdateForTest;
            imageUpdateDto.Id = this.testImage.Id;

            var response = await this.Client.UpdateAsync(imageUpdateDto, this.TokenStorage.AdminAccessToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestImage]
        public async Task Update_IncorrectId_ReturnBadRequest()
        {
            var imageUpdateDto = ExtractCreateTestImageAttribute.ImageFileUpdateForTest;
            int nonExistentId = -100;
            imageUpdateDto.Id = nonExistentId;

            var response = await this.Client.UpdateAsync(imageUpdateDto, this.TokenStorage.AdminAccessToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnSuccessStatusCode()
        {
            Image expectedImageToDelete = this.testImage;

            var response = await this.Client.DeleteAsync(expectedImageToDelete.Id, this.TokenStorage.AdminAccessToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_IncorrectId_ReturnBadRequest()
        {
            int nonExistentId = -100;

            var response = await this.Client.DeleteAsync(nonExistentId, this.TokenStorage.AdminAccessToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StreetcodeContentExtracter.Remove(this.testStreetcodeContent);
                ImageExtracter.Remove(this.testImage);
            }

            base.Dispose(disposing);
        }
    }
}
