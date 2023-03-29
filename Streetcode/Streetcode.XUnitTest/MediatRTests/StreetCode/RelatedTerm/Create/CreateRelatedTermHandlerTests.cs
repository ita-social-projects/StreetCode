using System;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Create;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
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


        [Theory]
        [InlineData(2, "example")]
        public async Task ShouldReturnSuccessfully_WhenRelatedTermAdded(int termId, string word)
        {
            // Arrange
            var relatedTermDTO = new RelatedTermDTO { TermId = termId, Word = word };
            var entity = new Entity { TermId = termId, Word = word };
            var createRelatedTermCommand = new CreateRelatedTermCommand(relatedTermDTO);


            _repositoryWrapperMock.Setup(m => m.RelatedTermRepository.Create(entity));
            _repositoryWrapperMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            _mapperMock.Setup(m => m.Map<Entity>(relatedTermDTO)).Returns(entity);

            var handler = new CreateRelatedTermHandler(_repositoryWrapperMock.Object, _mapperMock.Object);


            // Act
            var result = await handler.Handle(createRelatedTermCommand, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _repositoryWrapperMock.Verify(r => r.RelatedTermRepository.Create(entity), Times.Once);
            _repositoryWrapperMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Error_When_Related_Term_Is_Null()
        {
            // Arrange
            _mapperMock.Setup(m => m.Map<Entity>(It.IsAny<RelatedTermDTO>())).Returns<Entity>(null);
            var handler = new CreateRelatedTermHandler(_repositoryWrapperMock.Object, _mapperMock.Object);
            var command = new CreateRelatedTermCommand(new RelatedTermDTO());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal("Cannot create new related word for a term!", result.Errors.First().Message);
        }

        [Theory]
        [InlineData(1, "test")]
        public async Task Handle_Should_Return_Error_When_Related_Term_Already_Exists(int termId, string word)
        {
            // Arrange
            var relatedTerm = new RelatedTermDTO { TermId = termId, Word = word };
            var entity = new Entity { TermId = relatedTerm.TermId, Word = relatedTerm.Word };
            var existingTerms = new List<Entity> { entity };

            _mapperMock.Setup(m => m.Map<Entity>(It.IsAny<RelatedTermDTO>())).Returns(entity);
            _repositoryWrapperMock.Setup(r => r.RelatedTermRepository.GetAllAsync(It.IsAny<Expression<Func<Entity, bool>>>(),
                It.IsAny<Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>>>()))
            .ReturnsAsync(existingTerms);
            var handler = new CreateRelatedTermHandler(_repositoryWrapperMock.Object, _mapperMock.Object);
            var command = new CreateRelatedTermCommand(relatedTerm);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal("Слово з цим визначенням уже існує", result.Errors.First().Message);
            _repositoryWrapperMock.Verify(r => r.RelatedTermRepository.Create(entity), Times.Never);
            _repositoryWrapperMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }


        [Theory]
        [InlineData(1, "test")]
        public async Task Handle_Should_Return_Error_When_SaveChangesAsync_Fails(int termId, string word)
        {
            // Arrange
            var relatedTerm = new RelatedTermDTO { TermId = termId, Word = word };
            var entity = new Entity { TermId = relatedTerm.TermId, Word = relatedTerm.Word };
            var existingTerms = new List<Entity>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<Entity>(It.IsAny<RelatedTermDTO>())).Returns(entity);
            _repositoryWrapperMock.Setup(r => r.RelatedTermRepository.GetAllAsync(It.IsAny<Expression<Func<Entity, bool>>>(),
                It.IsAny<Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>>>()))
            .ReturnsAsync(existingTerms);
            repositoryMock.Setup(r => r.RelatedTermRepository.Create(It.IsAny<Entity>()));
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(0);
            var handler = new CreateRelatedTermHandler(repositoryMock.Object, mapperMock.Object);
            var command = new CreateRelatedTermCommand(relatedTerm);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal("Cannot save changes in the database after related word creation!", result.Errors.First().Message);
            repositoryMock.Verify(r => r.RelatedTermRepository.Create(entity), Times.Once);
            repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }







}

