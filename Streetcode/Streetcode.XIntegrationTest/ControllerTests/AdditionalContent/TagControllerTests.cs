using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using TagEntity = Streetcode.DAL.Entities.AdditionalContent.Tag;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Tag;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Tag;
using System.Net;
using Moq;
using Xunit;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Create;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Update;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using AutoMapper.Execution;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Delete;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent.Tag
{
    [Collection("Authorization")]
    public class TagControllerTests : BaseAuthorizationControllerTests<TagClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private TagEntity _testCreateTag;
        private TagEntity _testUpdateTag;
        private StreetcodeContent _testStreetcodeContent;

        public TagControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
            : base(factory, "/api/Tag", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.Generate();
            this._testCreateTag = TagExtracter.Extract(uniqueId, Guid.NewGuid().ToString());
            this._testUpdateTag = TagExtracter.Extract(uniqueId, Guid.NewGuid().ToString());
            this._testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    uniqueId,
                    uniqueId,
                    Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
            TagExtracter.Remove(this._testCreateTag);
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await this.client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<TagDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetAll_ReturnBadRequest()
        {
            var repositoryMock = new Mock<ITagRepository>();
            repositoryMock.Setup(repo => repo.GetAllAsync(default, default)).ReturnsAsync((IEnumerable<TagEntity>)null);

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.SetupGet(wrapper => wrapper.TagRepository)
                .Returns(repositoryMock.Object);

            var mapperMock = new Mock<IMapper>();
            var stringLocalizerMock = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            stringLocalizerMock.Setup(x => x["CannotFindAnyTags"]).Returns(new LocalizedString("CannotFindAnyTags", "Error message"));
            var loggerMock = new Mock<ILoggerService>();

            var handler = new GetAllTagsHandler(repositoryWrapperMock.Object, mapperMock.Object, loggerMock.Object, stringLocalizerMock.Object);

            var query = new GetAllTagsQuery();
            var cancellationToken = CancellationToken.None;

            var result = await handler.Handle(query, cancellationToken);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            TagEntity expectedTag = this._testCreateTag;
            var response = await this.client.GetByIdAsync(expectedTag.Id);

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
            var response = await this.client.GetByIdAsync(incorrectId);

            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            TagExtracter.AddStreetcodeTagIndex(this._testStreetcodeContent.Id, this._testCreateTag.Id);
            int streetcodeId = this._testStreetcodeContent.Id;
            var response = await client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeTagDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnBadRequest()
        {
            TagExtracter.AddStreetcodeTagIndex(this._testStreetcodeContent.Id, this._testCreateTag.Id);
            int streetcodeId = -100;
            var response = await client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeTagDTO>>(response.Content);

            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetByTagId_Incorrect_ReturnBadRequest()
        {
            int tagId = -100;
            var response = await this.client.GetByIdAsync(tagId);

            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetByTitle_ReturnSuccessStatusCode()
        {

            TagEntity expectedTag = this._testCreateTag;
            var response = await this.client.GetTagByTitle(expectedTag.Title);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TagDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Equal(expectedTag.Title, returnedValue?.Title);
        }

        [Fact]
        public async Task GetByTitle_Incorrect_ReturnBadRequest()
        {
            string title = "Some_Incorrect_Title";
            var response = await this.client.GetTagByTitle(title);

            Assert.Multiple(
              () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
              () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        [ExtractCreateTestTag]
        public async Task Create_ReturnsSuccessStatusCode()
        {
            // Arrange
            var tagCreateDTO = ExtractCreateTestTag.TagForTest;

            // Act
            var response = await client.CreateAsync(tagCreateDTO, _tokenStorage.AdminToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestTag]
        public async Task Create_ChangesNotSaved_ReturnsBadRequest()
        {

            var tagCreateDTO = ExtractCreateTestTag.TagForTest;

            var repositoryMock = new Mock<ITagRepository>();
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(default, default)).ReturnsAsync((TagEntity)null);
            repositoryMock.Setup(r => r.CreateAsync(default)).ReturnsAsync(this._testCreateTag);
            repositoryWrapperMock.SetupGet(wrapper => wrapper.TagRepository).Returns(repositoryMock.Object);
            repositoryWrapperMock.Setup(wrapper => wrapper.SaveChanges()).Returns(null);
            repositoryWrapperMock.Setup(wrapper => wrapper.SaveChanges()).Throws(default(Exception));

            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILoggerService>();

            var handler = new CreateTagHandler(repositoryWrapperMock.Object, mapperMock.Object, loggerMock.Object);

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
            var tagCreateDTO = ExtractCreateTestTag.TagForTest;

            // Act
            var response = await client.CreateAsync(tagCreateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestTag]
        public async Task Create_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Arrange
            var tagCreateDTO = ExtractCreateTestTag.TagForTest;

            // Act
            var response = await client.CreateAsync(tagCreateDTO, _tokenStorage.UserToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }


        [Fact]
        [ExtractCreateTestTag]
        public async Task Create_CreatesNewTag()
        {
            // Arrange
            var tagCreateDTO = ExtractCreateTestTag.TagForTest;

            // Act
            var response = await client.CreateAsync(tagCreateDTO, _tokenStorage.AdminToken);
            var getResponse = await client.GetTagByTitle(tagCreateDTO.Title);
            var fetchedStreetcode = CaseIsensitiveJsonDeserializer.Deserialize<TagEntity>(getResponse.Content);

            // Assert
            Assert.Equal(tagCreateDTO.Title, fetchedStreetcode.Title);
        }

        [Fact]
        [ExtractCreateTestTag]
        public async Task Create_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var tagCreateDTO = ExtractCreateTestTag.TagForTest;
            tagCreateDTO.Title = null;  // Invalid data

            // Act
            var response = await client.CreateAsync(tagCreateDTO, _tokenStorage.AdminToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestTag]
        public async Task Create_WithExistingTag_ReturnsConflict()
        {
            // Arrange
            var tagCreateDTO = ExtractCreateTestTag.TagForTest;
            tagCreateDTO.Title = _testCreateTag.Title;

            // Act
            var response = await client.CreateAsync(tagCreateDTO, _tokenStorage.AdminToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTestTag]
        public async Task Update_ReturnSuccessStatusCode()
        {
            // Arrange
            var tagUpdateDTO = ExtractUpdateTestTag.TagForTest;
            tagUpdateDTO.Id = this._testCreateTag.Id;

            // Act
            var response = await client.UpdateAsync(tagUpdateDTO, _tokenStorage.AdminToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTestTag]
        public async Task Update_Incorect_ReturnBadRequest()
        {
            // Arrange
            var tagUpdateDTO = ExtractUpdateTestTag.TagForTest;

            // Act
            var response = await client.UpdateAsync(tagUpdateDTO, _tokenStorage.AdminToken);

            // Assert
            Assert.Multiple(
               () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
               () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        [ExtractUpdateTestTag]
        public async Task Update_TokenNotPassed_ReturnsUnauthorized()
        {
            // Arrange
            var tagUpdateDTO = ExtractUpdateTestTag.TagForTest;

            // Act
            var response = await client.UpdateAsync(tagUpdateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTestTag]
        public async Task Update_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Arrange
            var tagUpdateDTO = ExtractUpdateTestTag.TagForTest;

            // Act
            var response = await client.UpdateAsync(tagUpdateDTO, _tokenStorage.UserToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTestTag]
        public async Task Update_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var tagUpdateDTO = ExtractUpdateTestTag.TagForTest;
            tagUpdateDTO.Title = null;  // Invalid data

            // Act
            var response = await client.UpdateAsync(tagUpdateDTO, _tokenStorage.AdminToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTestTag]
        public async Task Update_WithExistingTitle_ReturnsBadRequest()
        {
            // Arrange
            var tagUpdateDTO = ExtractUpdateTestTag.TagForTest;
            tagUpdateDTO.Id = this._testCreateTag.Id;
            tagUpdateDTO.Title = this._testUpdateTag.Title;

            // Act
            var response = await client.UpdateAsync(tagUpdateDTO, _tokenStorage.AdminToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTestTag]
        public async Task Update_ChangesNotSaved_ReturnsBadRequest()
        {
            // Arrange
            Expression<Func<TagEntity, bool>> testExpression = x => x.Id == this._testUpdateTag.Id;

            var tagUpdateDTO = ExtractUpdateTestTag.TagForTest;
            tagUpdateDTO.Id = this._testUpdateTag.Id;

            var repositoryMock = new Mock<ITagRepository>();
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<TagEntity, bool>>>(), default))
            .ReturnsAsync((Expression<Func<TagEntity, bool>> expr, IIncludableQueryable<TagEntity, bool> include) =>
            {
                var compiledExpr = expr.Compile();
                return compiledExpr(this._testUpdateTag) ? this._testUpdateTag : null;
            });

            repositoryWrapperMock.SetupGet(wrapper => wrapper.TagRepository).Returns(repositoryMock.Object);
            repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).ReturnsAsync(null);
            repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).Throws(default(Exception));

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<TagEntity>(default)).Returns(this._testUpdateTag);
            var loggerMock = new Mock<ILoggerService>();

            var handler = new UpdateTagHandler(repositoryWrapperMock.Object, mapperMock.Object, loggerMock.Object);

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
            int id = this._testCreateTag.Id;

            // Act
            var response = await this.client.Delete(id, this._tokenStorage.AdminToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_TokenNotPassed_ReturnsUnathorized()
        {
            // Arrange
            int id = this._testCreateTag.Id;

            // Act
            var response = await this.client.Delete(id);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Delete_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Arrange
            int id = this._testCreateTag.Id;

            // Act
            var response = await this.client.Delete(id, this._tokenStorage.UserToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Delete_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            int id = -100;

            // Act
            var response = await this.client.Delete(id, this._tokenStorage.AdminToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ChangesNotSaved_ReturnsBadRequest()
        {
            int id = this._testUpdateTag.Id;

            var repositoryMock = new Mock<ITagRepository>();
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<TagEntity, bool>>>(), default))
            .ReturnsAsync(this._testUpdateTag);
            repositoryMock.Setup(r => r.Delete(default));

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
    }
}
