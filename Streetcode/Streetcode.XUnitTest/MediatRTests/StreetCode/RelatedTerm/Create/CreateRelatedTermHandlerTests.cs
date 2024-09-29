using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using Entity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.RelatedTerm.Create
{
    public class CreateRelatedTermHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotCreateSharedResource>> mockLocalizerCannotCreate;
        private readonly Mock<IStringLocalizer<CannotMapSharedResource>> mockLocalizerCannotMap;
        private readonly Mock<IStringLocalizer<CreateRelatedTermHandler>> mockLocalizer;
        private readonly Mock<IStringLocalizer<CannotSaveSharedResource>> mockLocalizerCannotSave;

        public CreateRelatedTermHandlerTests()
        {
            this.repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            this.mapperMock = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CreateRelatedTermHandler>>();
            this.mockLocalizerCannotCreate = new Mock<IStringLocalizer<CannotCreateSharedResource>>();
            this.mockLocalizerCannotMap = new Mock<IStringLocalizer<CannotMapSharedResource>>();
            this.mockLocalizerCannotSave = new Mock<IStringLocalizer<CannotSaveSharedResource>>();
        }

        [Theory]
        [InlineData(2, "example")]
        public async Task ShouldReturnSuccessfully_WhenRelatedTermAdded(int termId, string word)
        {
            // Arrange
            var (relatedTermDTO, entity) = this.CreateRelatedTermObjects(termId, word);
            var createRelatedTermCommand = new CreateRelatedTermCommand(relatedTermDTO);

            this.repositoryWrapperMock.Setup(r => r.RelatedTermRepository.Create(entity));
            this.repositoryWrapperMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            this.SetupMapperMockToMapEntity(entity);
            this.SetupLocalizers();

            var handler = new CreateRelatedTermHandler(this.repositoryWrapperMock.Object, this.mapperMock.Object, this.mockLogger.Object, this.mockLocalizerCannotSave.Object, this.mockLocalizerCannotMap.Object, this.mockLocalizer.Object, this.mockLocalizerCannotCreate.Object);

            // Act
            var result = await handler.Handle(createRelatedTermCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            this.repositoryWrapperMock.Verify(r => r.RelatedTermRepository.Create(entity), Times.Once);
            this.repositoryWrapperMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Error_When_Related_Term_Is_Null()
        {
            // Arrange
            this.SetupMapperMockToMapEntity(null);
            var handler = new CreateRelatedTermHandler(this.repositoryWrapperMock.Object, this.mapperMock.Object, this.mockLogger.Object, this.mockLocalizerCannotSave.Object, this.mockLocalizerCannotMap.Object, this.mockLocalizer.Object, this.mockLocalizerCannotCreate.Object);
            var command = new CreateRelatedTermCommand(new RelatedTermCreateDTO());
            this.SetupLocalizers();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData(1, "test")]
        public async Task Handle_Should_Return_Error_When_Related_Term_Already_Exists(int termId, string word)
        {
            // Arrange
            var (relatedTermDTO, entity) = this.CreateRelatedTermObjects(termId, word);
            var existingTerms = new List<Entity> { entity };

            this.SetupMapperMockToMapEntity(entity);
            this.SetupGetAllAsyncWithExistingTerms(existingTerms);
            this.SetupLocalizers();

            var handler = new CreateRelatedTermHandler(this.repositoryWrapperMock.Object, this.mapperMock.Object, this.mockLogger.Object, this.mockLocalizerCannotSave.Object, this.mockLocalizerCannotMap.Object, this.mockLocalizer.Object, this.mockLocalizerCannotCreate.Object);
            var command = new CreateRelatedTermCommand(relatedTermDTO);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.False(result.IsSuccess);
            this.VerifyCreateAndSaveChangesNever();
        }

        [Theory]
        [InlineData(1, "test")]
        public async Task Handle_Should_Return_Error_When_SaveChangesAsync_Fails(int termId, string word)
        {
            // Arrange
            var (relatedTermDTO, entity) = this.CreateRelatedTermObjects(termId, word);
            var existingTerms = new List<Entity>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            var mapperMock = new Mock<IMapper>();

            mapperMock.Setup(m => m.Map<Entity>(It.IsAny<RelatedTermDTO>())).Returns(entity);
            this.SetupGetAllAsyncWithExistingTerms(existingTerms);

            repositoryMock.Setup(r => r.RelatedTermRepository.Create(It.IsAny<Entity>()));
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(0);
            this.SetupLocalizers();

            var handler = new CreateRelatedTermHandler(repositoryMock.Object, mapperMock.Object, this.mockLogger.Object, this.mockLocalizerCannotSave.Object, this.mockLocalizerCannotMap.Object, this.mockLocalizer.Object, this.mockLocalizerCannotCreate.Object);
            var command = new CreateRelatedTermCommand(relatedTermDTO);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.False(result.IsSuccess);
        }

        private void SetupLocalizers()
        {
            // Setup for _mockLocalizer
            this.mockLocalizer.Setup(x => x[It.IsAny<string>()])
                .Returns((string key) => new LocalizedString(key, $"Word with this definition already exists"));

            // Setup for _mockLocalizerCannotCreate
            this.mockLocalizerCannotCreate.Setup(x => x[It.IsAny<string>()])
                .Returns((string key) => new LocalizedString(key, $"Cannot create new related word for a term"));

            // Setup for _mockLocalizerCannotMap
            this.mockLocalizerCannotMap.Setup(x => x[It.IsAny<string>()])
                .Returns((string key) => new LocalizedString(key, $"Cannot map entity"));

            // Setup for _mockLocalizerCannotSave
            this.mockLocalizerCannotSave.Setup(x => x[It.IsAny<string>()])
                .Returns((string key) => new LocalizedString(key, $"Cannot save changes in the database after related word creation"));
        }

        private (RelatedTermCreateDTO relatedTermDTO, Entity entity) CreateRelatedTermObjects(int termId, string word)
        {
            var relatedTermDTO = new RelatedTermCreateDTO { TermId = termId, Word = word };
            var entity = new Entity { TermId = relatedTermDTO.TermId, Word = relatedTermDTO.Word };

            return (relatedTermDTO, entity);
        }

        private void SetupMapperMockToMapEntity(Entity? entity)
        {
            this.mapperMock.Setup(m => m.Map<Entity?>(It.IsAny<RelatedTermCreateDTO>())).Returns(entity);
        }

        private void SetupGetAllAsyncWithExistingTerms(List<Entity> existingTerms)
        {
            this.repositoryWrapperMock
                .Setup(r => r.RelatedTermRepository
                    .GetAllAsync(
                        It.IsAny<Expression<Func<Entity, bool>>>(),
                        It.IsAny<Func<IQueryable<Entity>,
                        IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(existingTerms);
        }

        private void VerifyCreateAndSaveChangesNever()
        {
            this.repositoryWrapperMock.Verify(r => r.RelatedTermRepository.Create(It.IsAny<Entity>()), Times.Never);
            this.repositoryWrapperMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}