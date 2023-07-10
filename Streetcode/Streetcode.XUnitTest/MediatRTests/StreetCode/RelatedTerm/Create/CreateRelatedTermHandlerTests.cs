using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Create;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Entity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.RelatedTerm.Create
{
    public class CreateRelatedTermHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
        private readonly Mock<IMapper> _mapperMock;

        public CreateRelatedTermHandlerTests()
        {
            _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            _mapperMock = new Mock<IMapper>();
        }

        private (RelatedTermDTO relatedTermDTO, Entity entity) CreateRelatedTermObjects(int termId, string word)
        {
            var relatedTermDTO = new RelatedTermDTO { TermId = termId, Word = word };
            var entity = new Entity { TermId = relatedTermDTO.TermId, Word = relatedTermDTO.Word };

            return (relatedTermDTO, entity);
        }

        private void SetupMapperMockToMapEntity(RelatedTermDTO relatedTermDTO, Entity entity)
        {
            _mapperMock.Setup(m => m.Map<Entity>(It.IsAny<RelatedTermDTO>())).Returns(entity);
        }

        private void SetupGetAllAsyncWithExistingTerms(List<Entity> existingTerms)
        {
            _repositoryWrapperMock.Setup(r => r.RelatedTermRepository
                .GetAllAsync(It.IsAny<Expression<Func<Entity, bool>>>(),
                    It.IsAny<Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(existingTerms);
        }

        private void VerifyCreateAndSaveChangesNever()
        {
            _repositoryWrapperMock.Verify(r => r.RelatedTermRepository.Create(It.IsAny<Entity>()), Times.Never);
            _repositoryWrapperMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        private void VerifyCreateAndSaveChangesOnce(Entity entity)
        {
            _repositoryWrapperMock.Verify(r => r.RelatedTermRepository.Create(entity), Times.Once);
            _repositoryWrapperMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(2, "example")]
        public async Task ShouldReturnSuccessfully_WhenRelatedTermAdded(int termId, string word)
        {
            // Arrange
            var (relatedTermDTO, entity) = CreateRelatedTermObjects(termId, word);
            var createRelatedTermCommand = new CreateRelatedTermCommand(relatedTermDTO);

            _repositoryWrapperMock.Setup(r => r.RelatedTermRepository.Create(entity));
            _repositoryWrapperMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            SetupMapperMockToMapEntity(relatedTermDTO, entity);

            var handler = new CreateRelatedTermHandler(_repositoryWrapperMock.Object, _mapperMock.Object);

            // Act
            var result = await handler.Handle(createRelatedTermCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            _repositoryWrapperMock.Verify(r => r.RelatedTermRepository.Create(entity), Times.Once);
            _repositoryWrapperMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Error_When_Related_Term_Is_Null()
        {
            // Arrange
            SetupMapperMockToMapEntity(It.IsAny<RelatedTermDTO>(), null);
            var handler = new CreateRelatedTermHandler(_repositoryWrapperMock.Object, _mapperMock.Object);
            var command = new CreateRelatedTermCommand(new RelatedTermDTO());

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
            var (relatedTermDTO, entity) = CreateRelatedTermObjects(termId, word);
            var existingTerms = new List<Entity> { entity };

            SetupMapperMockToMapEntity(relatedTermDTO, entity);
            SetupGetAllAsyncWithExistingTerms(existingTerms);

            var handler = new CreateRelatedTermHandler(_repositoryWrapperMock.Object, _mapperMock.Object);
            var command = new CreateRelatedTermCommand(relatedTermDTO);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.False(result.IsSuccess);
            VerifyCreateAndSaveChangesNever();
        }

        [Theory]
        [InlineData(1, "test")]
        public async Task Handle_Should_Return_Error_When_SaveChangesAsync_Fails(int termId, string word)
        {
            // Arrange
            var (relatedTermDTO, entity) = CreateRelatedTermObjects(termId, word);
            var existingTerms = new List<Entity>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            var mapperMock = new Mock<IMapper>();

            mapperMock.Setup(m => m.Map<Entity>(It.IsAny<RelatedTermDTO>())).Returns(entity);
            SetupGetAllAsyncWithExistingTerms(existingTerms);

            repositoryMock.Setup(r => r.RelatedTermRepository.Create(It.IsAny<Entity>()));
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(0);

            var handler = new CreateRelatedTermHandler(repositoryMock.Object, mapperMock.Object);
            var command = new CreateRelatedTermCommand(relatedTermDTO);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.False(result.IsSuccess);
        }
    }
}