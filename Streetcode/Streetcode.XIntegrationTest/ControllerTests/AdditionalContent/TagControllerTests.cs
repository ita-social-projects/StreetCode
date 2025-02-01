using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Create;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Delete;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Tag;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Tag;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;
using TagEntity = Streetcode.DAL.Entities.AdditionalContent.Tag;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent.Tag
{
    [Collection("Authorization")]
    public class TagControllerTests : BaseAuthorizationControllerTests<TagClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly TagEntity testCreateTag;
        private readonly TagEntity testUpdateTag;
        private readonly StreetcodeContent testStreetcodeContent;

        public TagControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
            : base(factory, "/api/Tag", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testCreateTag = TagExtracter.Extract(uniqueId, Guid.NewGuid().ToString());
            this.testUpdateTag = TagExtracter.Extract(uniqueId, Guid.NewGuid().ToString());
            this.testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    uniqueId,
                    uniqueId,
                    Guid.NewGuid().ToString());
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await this.Client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<GetAllTagsResponseDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            TagEntity expectedTag = this.testCreateTag;
            var response = await this.Client.GetByIdAsync(expectedTag.Id);

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TagDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Multiple(
                () => Assert.Equal(expectedTag.Id, returnedValue?.Id),
                () => Assert.Equal(expectedTag.Title, returnedValue?.Title));
        }

        [Fact]
        public async Task GetByIdIncorrect_ReturnBadRequest()
        {
            int incorrectId = -100;
            var response = await this.Client.GetByIdAsync(incorrectId);

            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            TagExtracter.AddStreetcodeTagIndex(this.testStreetcodeContent.Id, this.testCreateTag.Id);
            int streetcodeId = this.testStreetcodeContent.Id;
            var response = await this.Client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeTagDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnBadRequest()
        {
            TagExtracter.AddStreetcodeTagIndex(this.testStreetcodeContent.Id, this.testCreateTag.Id);
            int streetcodeId = -100;
            var response = await this.Client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeTagDTO>>(response.Content);

            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode),
                () => Assert.NotNull(returnedValue));
        }

        [Fact]
        public async Task GetByTagId_Incorrect_ReturnBadRequest()
        {
            int tagId = -100;
            var response = await this.Client.GetByIdAsync(tagId);

            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetByTitle_ReturnSuccessStatusCode()
        {
            TagEntity expectedTag = this.testCreateTag;
            var response = await this.Client.GetTagByTitle(expectedTag.Title);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TagDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Equal(expectedTag.Title, returnedValue.Title);
        }

        [Fact]
        public async Task GetByTitle_Incorrect_ReturnBadRequest()
        {
            string title = "Some_Incorrect_Title";
            var response = await this.Client.GetTagByTitle(title);

            Assert.Multiple(
              () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
              () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        [ExtractCreateTestTag]
        public async Task Create_ReturnsSuccessStatusCode()
        {
            // Arrange
            var tagCreateDTO = ExtractCreateTestTagAttribute.TagForTest;

            // Act
            var response = await this.Client.CreateAsync(tagCreateDTO, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestTag]
        public async Task Create_ChangesNotSaved_ReturnsBadRequest()
        {
            var tagCreateDTO = ExtractCreateTestTagAttribute.TagForTest;

            var repositoryMock = new Mock<ITagRepository>();
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            var mockStringLocalizerFailedToValidate = new Mock<IStringLocalizer<FailedToValidateSharedResource>>();
            var mockStringLocalizerFieldNames = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
            repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(default, default)).ReturnsAsync((TagEntity?)null);
            repositoryMock.Setup(r => r.CreateAsync(default!)).ReturnsAsync(this.testCreateTag);
            repositoryWrapperMock.SetupGet(wrapper => wrapper.TagRepository).Returns(repositoryMock.Object);
            repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).ReturnsAsync(null);
            repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).Throws(default(Exception));

            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILoggerService>();

            var handler = new CreateTagHandler(
                repositoryWrapperMock.Object,
                mapperMock.Object,
                loggerMock.Object,
                mockStringLocalizerFailedToValidate.Object,
                mockStringLocalizerFieldNames.Object);

            var query = new CreateTagQuery(tagCreateDTO);
            var cancellationToken = CancellationToken.None;

            var result = await handler.Handle(query, cancellationToken);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        [ExtractCreateTestTag]
        public async Task Create_TokenNotPassed_ReturnsUnauthorized()
        {
            // Arrange
            var tagCreateDTO = ExtractCreateTestTagAttribute.TagForTest;

            // Act
            var response = await this.Client.CreateAsync(tagCreateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestTag]
        public async Task Create_NotAdminAccessTokenPassed_ReturnsForbidden()
        {
            // Arrange
            var tagCreateDTO = ExtractCreateTestTagAttribute.TagForTest;

            // Act
            var response = await this.Client.CreateAsync(tagCreateDTO, this.TokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestTag]
        public async Task Create_CreatesNewTag()
        {
            // Arrange
            var tagCreateDTO = ExtractCreateTestTagAttribute.TagForTest;

            // Act
            await this.Client.CreateAsync(tagCreateDTO, this.TokenStorage.AdminAccessToken);
            var getResponse = await this.Client.GetTagByTitle(tagCreateDTO.Title);
            var fetchedStreetcode = CaseIsensitiveJsonDeserializer.Deserialize<TagEntity>(getResponse.Content);

            // Assert
            Assert.Equal(tagCreateDTO.Title, fetchedStreetcode?.Title);
        }

        [Fact]
        [ExtractCreateTestTag]
        public async Task Create_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var tagCreateDTO = ExtractCreateTestTagAttribute.TagForTest;
            tagCreateDTO.Title = null!;  // Invalid data

            // Act
            var response = await this.Client.CreateAsync(tagCreateDTO, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestTag]
        public async Task Create_WithExistingTag_ReturnsConflict()
        {
            // Arrange
            var tagCreateDTO = ExtractCreateTestTagAttribute.TagForTest;
            tagCreateDTO.Title = this.testCreateTag.Title;

            // Act
            var response = await this.Client.CreateAsync(tagCreateDTO, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTestTagAttribute]
        public async Task Update_ReturnSuccessStatusCode()
        {
            // Arrange
            var tagUpdateDTO = ExtractUpdateTestTagAttribute.TagForTest;
            tagUpdateDTO.Id = this.testCreateTag.Id;

            // Act
            var response = await this.Client.UpdateAsync(tagUpdateDTO, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTestTagAttribute]
        public async Task Update_Incorect_ReturnBadRequest()
        {
            // Arrange
            var tagUpdateDTO = ExtractUpdateTestTagAttribute.TagForTest;
            var incorrectTagId = -10;
            tagUpdateDTO.Id = incorrectTagId;

            // Act
            var response = await this.Client.UpdateAsync(tagUpdateDTO, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Multiple(
               () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
               () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        [ExtractUpdateTestTagAttribute]
        public async Task Update_TokenNotPassed_ReturnsUnauthorized()
        {
            // Arrange
            var tagUpdateDTO = ExtractUpdateTestTagAttribute.TagForTest;

            // Act
            var response = await this.Client.UpdateAsync(tagUpdateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTestTagAttribute]
        public async Task Update_NotAdminAccessTokenPassed_ReturnsForbidden()
        {
            // Arrange
            var tagUpdateDTO = ExtractUpdateTestTagAttribute.TagForTest;

            // Act
            var response = await this.Client.UpdateAsync(tagUpdateDTO, this.TokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTestTagAttribute]
        public async Task Update_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var tagUpdateDTO = ExtractUpdateTestTagAttribute.TagForTest;
            tagUpdateDTO.Title = null!;  // Invalid data

            // Act
            var response = await this.Client.UpdateAsync(tagUpdateDTO, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTestTagAttribute]
        public async Task Update_WithExistingTitle_ReturnsBadRequest()
        {
            // Arrange
            var tagUpdateDTO = ExtractUpdateTestTagAttribute.TagForTest;
            tagUpdateDTO.Id = this.testCreateTag.Id - 1;
            tagUpdateDTO.Title = this.testUpdateTag.Title;

            // Act
            var response = await this.Client.UpdateAsync(tagUpdateDTO, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTestTagAttribute]
        public async Task Update_ChangesNotSaved_ReturnsBadRequest()
        {
            // Arrange
            var tagUpdateDTO = ExtractUpdateTestTagAttribute.TagForTest;
            tagUpdateDTO.Id = this.testUpdateTag.Id;

            var repositoryMock = new Mock<ITagRepository>();
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            var mockStringLocalizerFailedToValidate = new Mock<IStringLocalizer<FailedToValidateSharedResource>>();
            var mockStringLocalizerFieldNames = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
            var mockStringLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<TagEntity, bool>>>(), default))
            .ReturnsAsync((Expression<Func<TagEntity, bool>> expr, IIncludableQueryable<TagEntity, bool> include) =>
            {
                var compiledExpr = expr.Compile();
                return compiledExpr(this.testUpdateTag) ? this.testUpdateTag : null;
            });

            repositoryWrapperMock.SetupGet(wrapper => wrapper.TagRepository).Returns(repositoryMock.Object);
            repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).ReturnsAsync(null);
            repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).Throws(default(Exception));

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<TagEntity>(default)).Returns(this.testUpdateTag);
            var loggerMock = new Mock<ILoggerService>();

            var handler = new UpdateTagHandler(
                repositoryWrapperMock.Object,
                mapperMock.Object,
                loggerMock.Object,
                mockStringLocalizerFailedToValidate.Object,
                mockStringLocalizerFieldNames.Object,
                mockStringLocalizerCannotFind.Object);

            var query = new UpdateTagCommand(tagUpdateDTO);
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await handler.Handle(query, cancellationToken);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Delete_ReturnsSuccessStatusCode()
        {
            // Arrange
            int id = this.testCreateTag.Id;

            // Act
            var response = await this.Client.Delete(id, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_TokenNotPassed_ReturnsUnathorized()
        {
            // Arrange
            int id = this.testCreateTag.Id;

            // Act
            var response = await this.Client.Delete(id);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Delete_NotAdminAccessTokenPassed_ReturnsForbidden()
        {
            // Arrange
            int id = this.testCreateTag.Id;

            // Act
            var response = await this.Client.Delete(id, this.TokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Delete_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            int id = -100;

            // Act
            var response = await this.Client.Delete(id, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ChangesNotSaved_ReturnsBadRequest()
        {
            int id = this.testUpdateTag.Id;

            var repositoryMock = new Mock<ITagRepository>();
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<TagEntity, bool>>>(), default))
            .ReturnsAsync(this.testUpdateTag);
            repositoryMock.Setup(r => r.Delete(default!));

            repositoryWrapperMock.SetupGet(wrapper => wrapper.TagRepository).Returns(repositoryMock.Object);
            repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).ReturnsAsync(null);
            repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).Throws(default(Exception));

            var loggerMock = new Mock<ILoggerService>();

            var handler = new DeleteTagHandler(repositoryWrapperMock.Object, loggerMock.Object);

            var query = new DeleteTagCommand(id);
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await handler.Handle(query, cancellationToken);

            // Assert
            Assert.False(result.IsSuccess);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StreetcodeContentExtracter.Remove(this.testStreetcodeContent);
                TagExtracter.Remove(this.testCreateTag);
            }

            base.Dispose(disposing);
        }
    }
}
