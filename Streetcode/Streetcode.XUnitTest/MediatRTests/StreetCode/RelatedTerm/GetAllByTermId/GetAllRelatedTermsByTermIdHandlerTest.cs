using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.GetAllByTermId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Entity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.RelatedTerm.GetAllByTermId
{
    public class GetAllRelatedTermsByTermIdHandlerTest
    {
        private Mock<IRepositoryWrapper> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private readonly Mock<IStringLocalizer<CannotGetSharedResource>> _mockLocalizerCannotGet;
        private readonly Mock<IStringLocalizer<CannotCreateSharedResource>> _mockLocalizerCannotCreate;

        public GetAllRelatedTermsByTermIdHandlerTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsError_WhenRepositoryReturnsNull(int id)
        {
            // Arrange
            var query = new GetAllRelatedTermsByTermIdQuery(id);

            _mockRepository.Setup(x => x.RelatedTermRepository
            .GetAllAsync(It.IsAny<Expression<Func<Entity, bool>>>(),
                It.IsAny<Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync((IEnumerable<Entity>)null);

            var handler = new GetAllRelatedTermsByTermIdHandler(_mockMapper.Object, _mockRepository.Object,_mockLocalizerCannotGet.Object,_mockLocalizerCannotCreate.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal("Cannot get words by term id", result.Errors.First().Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsError_WhenMapperReturnsNull(int id)
        {
            // Arrange
            var query = new GetAllRelatedTermsByTermIdQuery(id);
            var relatedTerms = new List<Entity> { new Entity() };

            _mockRepository.Setup(x => x.RelatedTermRepository.
            GetAllAsync(It.IsAny<Expression<Func<Entity, bool>>>(),
                It.IsAny<Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(relatedTerms);
            _mockMapper.Setup(x => x.Map<IEnumerable<RelatedTermDTO>>(relatedTerms))
                .Returns((IEnumerable<RelatedTermDTO>)null);

            var handler = new GetAllRelatedTermsByTermIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLocalizerCannotGet.Object, _mockLocalizerCannotCreate.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal("Cannot create DTOs for related words!", result.Errors.First().Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsOkResult_WhenAllInputsAreValid(int id)
        {
            // Arrange
            var query = new GetAllRelatedTermsByTermIdQuery(id);

            var relatedTerms = new List<Entity> { new Entity() };

            var relatedTermDTOs = new List<RelatedTermDTO> { new RelatedTermDTO() };

            _mockRepository.Setup(x => x.RelatedTermRepository.GetAllAsync(It.IsAny<Expression<Func<Entity, bool>>>(),
                It.IsAny<Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(relatedTerms);
            _mockMapper.Setup(x => x.Map<IEnumerable<RelatedTermDTO>>(relatedTerms)).Returns(relatedTermDTOs);

            var handler = new GetAllRelatedTermsByTermIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLocalizerCannotGet.Object, _mockLocalizerCannotCreate.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(
              () => Assert.True(result.IsSuccess),
              () => Assert.Equal(relatedTermDTOs, result.Value)
              );
        }

        [Theory]
        [InlineData(1, 1, "RelatedTerm1")]
        public async Task Handle_WhenRelatedTermsFound_ReturnsSuccessResult(int id, int termId, string testWord)
        {
            // Arrange
            var relatedTerm = CreateNewEntity(id, testWord, termId);
            var relatedTerms = new List<Entity> { relatedTerm };
            var relatedTermDTOs = new List<RelatedTermDTO> { new RelatedTermDTO() };

            _mockMapper.Setup(x => x.Map<IEnumerable<RelatedTermDTO>>(relatedTerms)).Returns(relatedTermDTOs);

            _mockRepository.Setup(x => x.RelatedTermRepository.GetAllAsync(It.IsAny<Expression<Func<Entity, bool>>>(),
                It.IsAny<Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(relatedTerms);

            var query = new GetAllRelatedTermsByTermIdQuery(termId);
            var handler = new GetAllRelatedTermsByTermIdHandler(_mockMapper.Object, _mockRepository.Object, _mockLocalizerCannotGet.Object, _mockLocalizerCannotCreate.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(
             () => Assert.True(result.IsSuccess),
             () => Assert.IsAssignableFrom<IEnumerable<RelatedTermDTO>>(result.Value),
             () => Assert.Equal(relatedTermDTOs.Count(), result.Value.Count()),
             () => Assert.Equal(relatedTermDTOs.First().Id, result.Value.First().Id),
             () => Assert.Equal(relatedTermDTOs.First().TermId, result.Value.First().TermId),
             () => Assert.Equal(relatedTermDTOs.First().Word, result.Value.First().Word)
                );
        }

        private static Entity CreateNewEntity(int id, string word, int termId)
        {
            return new Entity { Id = id, Word = word, TermId = termId };
        }
    }
}
