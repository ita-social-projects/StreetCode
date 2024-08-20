using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.GetAllByTermId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using Entity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.RelatedTerm.GetAllByTermId
{
    public class GetAllRelatedTermsByTermIdHandlerTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotGetSharedResource>> _mockLocalizerCannotGet;
        private readonly Mock<IStringLocalizer<CannotCreateSharedResource>> _mockLocalizerCannotCreate;

        public GetAllRelatedTermsByTermIdHandlerTest()
        {
            this._mockRepository = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerCannotCreate = new Mock<IStringLocalizer<CannotCreateSharedResource>>();
            this._mockLocalizerCannotGet = new Mock<IStringLocalizer<CannotGetSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsError_WhenRepositoryReturnsNull(int id)
        {
            // Arrange
            var query = new GetAllRelatedTermsByTermIdQuery(id);

            this._mockRepository
                .Setup(x => x.RelatedTermRepository
                    .GetAllAsync(
                        It.IsAny<Expression<Func<Entity, bool>>>(),
                        It.IsAny<Func<IQueryable<Entity>,
                        IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(new List<Entity>());

            var handler = new GetAllRelatedTermsByTermIdHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerCannotGet.Object, this._mockLocalizerCannotCreate.Object);
            var expectedError = "Cannot get words by term id";
            this._mockLocalizerCannotGet.Setup(x => x["CannotGetWordsByTermId"])
           .Returns(new LocalizedString("CannotGetWordsByTermId", expectedError));

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsError_WhenMapperReturnsNull(int id)
        {
            // Arrange
            var query = new GetAllRelatedTermsByTermIdQuery(id);
            var relatedTerms = new List<Entity> { new Entity() };

            this._mockRepository
                .Setup(x => x.RelatedTermRepository.
                    GetAllAsync(
                        It.IsAny<Expression<Func<Entity, bool>>>(),
                        It.IsAny<Func<IQueryable<Entity>,
                        IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(relatedTerms);
            this._mockMapper
                .Setup(x => x.Map<IEnumerable<RelatedTermDTO>>(relatedTerms))
                .Returns(new List<RelatedTermDTO>());

            var handler = new GetAllRelatedTermsByTermIdHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerCannotGet.Object, this._mockLocalizerCannotCreate.Object);
            var expectedError = "Cannot create DTOs for related words!";
            this._mockLocalizerCannotCreate.Setup(x => x["CannotCreateDTOsForRelatedWords"])
           .Returns(new LocalizedString("CannotCreateDTOsForRelatedWords", expectedError));

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsOkResult_WhenAllInputsAreValid(int id)
        {
            // Arrange
            var query = new GetAllRelatedTermsByTermIdQuery(id);

            var relatedTerms = new List<Entity> { new Entity() };

            var relatedTermDTOs = new List<RelatedTermDTO> { new RelatedTermDTO() };

            this._mockRepository
                .Setup(x => x.RelatedTermRepository
                    .GetAllAsync(
                        It.IsAny<Expression<Func<Entity, bool>>>(),
                        It.IsAny<Func<IQueryable<Entity>,
                        IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(relatedTerms);
            this._mockMapper.Setup(x => x.Map<IEnumerable<RelatedTermDTO>>(relatedTerms)).Returns(relatedTermDTOs);

            var handler = new GetAllRelatedTermsByTermIdHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerCannotGet.Object, this._mockLocalizerCannotCreate.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(
              () => Assert.True(result.IsSuccess),
              () => Assert.Equal(relatedTermDTOs, result.Value));
        }

        [Theory]
        [InlineData(1, 1, "RelatedTerm1")]
        public async Task Handle_WhenRelatedTermsFound_ReturnsSuccessResult(int id, int termId, string testWord)
        {
            // Arrange
            var relatedTerm = CreateNewEntity(id, testWord, termId);
            var relatedTerms = new List<Entity> { relatedTerm };
            var relatedTermDTOs = new List<RelatedTermDTO> { new RelatedTermDTO() };

            this._mockMapper.Setup(x => x.Map<IEnumerable<RelatedTermDTO>>(relatedTerms)).Returns(relatedTermDTOs);

            this._mockRepository
                .Setup(x => x.RelatedTermRepository
                    .GetAllAsync(
                        It.IsAny<Expression<Func<Entity, bool>>>(),
                        It.IsAny<Func<IQueryable<Entity>,
                        IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(relatedTerms);

            var query = new GetAllRelatedTermsByTermIdQuery(termId);
            var handler = new GetAllRelatedTermsByTermIdHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerCannotGet.Object, this._mockLocalizerCannotCreate.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(
             () => Assert.True(result.IsSuccess),
             () => Assert.IsAssignableFrom<IEnumerable<RelatedTermDTO>>(result.Value),
             () => Assert.Equal(relatedTermDTOs.Count, result.Value.Count()),
             () => Assert.Equal(relatedTermDTOs[0].Id, result.Value.First().Id),
             () => Assert.Equal(relatedTermDTOs[0].TermId, result.Value.First().TermId),
             () => Assert.Equal(relatedTermDTOs[0].Word, result.Value.First().Word));
        }

        private static Entity CreateNewEntity(int id, string word, int termId)
        {
            return new Entity { Id = id, Word = word, TermId = termId };
        }
    }
}
