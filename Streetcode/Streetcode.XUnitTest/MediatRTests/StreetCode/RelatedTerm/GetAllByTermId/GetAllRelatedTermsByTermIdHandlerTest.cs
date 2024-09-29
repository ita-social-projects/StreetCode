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
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotGetSharedResource>> mockLocalizerCannotGet;
        private readonly Mock<IStringLocalizer<CannotCreateSharedResource>> mockLocalizerCannotCreate;

        public GetAllRelatedTermsByTermIdHandlerTest()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotCreate = new Mock<IStringLocalizer<CannotCreateSharedResource>>();
            this.mockLocalizerCannotGet = new Mock<IStringLocalizer<CannotGetSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsError_WhenRepositoryReturnsNull(int id)
        {
            // Arrange
            var query = new GetAllRelatedTermsByTermIdQuery(id);

            this.mockRepository
                .Setup(x => x.RelatedTermRepository
                    .GetAllAsync(
                        It.IsAny<Expression<Func<Entity, bool>>>(),
                        It.IsAny<Func<IQueryable<Entity>,
                        IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(new List<Entity>());

            var handler = new GetAllRelatedTermsByTermIdHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotGet.Object, this.mockLocalizerCannotCreate.Object);
            var expectedError = "Cannot get words by term id";
            this.mockLocalizerCannotGet.Setup(x => x["CannotGetWordsByTermId"])
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

            this.mockRepository
                .Setup(x => x.RelatedTermRepository.
                    GetAllAsync(
                        It.IsAny<Expression<Func<Entity, bool>>>(),
                        It.IsAny<Func<IQueryable<Entity>,
                        IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(relatedTerms);
            this.mockMapper
                .Setup(x => x.Map<IEnumerable<RelatedTermDTO>>(relatedTerms))
                .Returns(new List<RelatedTermDTO>());

            var handler = new GetAllRelatedTermsByTermIdHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotGet.Object, this.mockLocalizerCannotCreate.Object);
            var expectedError = "Cannot create DTOs for related words!";
            this.mockLocalizerCannotCreate.Setup(x => x["CannotCreateDTOsForRelatedWords"])
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

            this.mockRepository
                .Setup(x => x.RelatedTermRepository
                    .GetAllAsync(
                        It.IsAny<Expression<Func<Entity, bool>>>(),
                        It.IsAny<Func<IQueryable<Entity>,
                        IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(relatedTerms);
            this.mockMapper.Setup(x => x.Map<IEnumerable<RelatedTermDTO>>(relatedTerms)).Returns(relatedTermDTOs);

            var handler = new GetAllRelatedTermsByTermIdHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotGet.Object, this.mockLocalizerCannotCreate.Object);

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

            this.mockMapper.Setup(x => x.Map<IEnumerable<RelatedTermDTO>>(relatedTerms)).Returns(relatedTermDTOs);

            this.mockRepository
                .Setup(x => x.RelatedTermRepository
                    .GetAllAsync(
                        It.IsAny<Expression<Func<Entity, bool>>>(),
                        It.IsAny<Func<IQueryable<Entity>,
                        IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(relatedTerms);

            var query = new GetAllRelatedTermsByTermIdQuery(termId);
            var handler = new GetAllRelatedTermsByTermIdHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotGet.Object, this.mockLocalizerCannotCreate.Object);

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
