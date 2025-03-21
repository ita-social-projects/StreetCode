using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.GetById;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.SubtitleTests
{
    public class GetSubtitleByIdRequestHandlerTests
    {
        private const int Id = 1;
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        private readonly Subtitle _subtitle = new Subtitle { Id = Id };
        private readonly SubtitleDTO _subtitleDto = new SubtitleDTO { Id = Id };

        public GetSubtitleByIdRequestHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handler_Returns_Matching_Element()
        {
            // Arrange
            SetupRepository(_subtitle);
            SetupMapper(_subtitleDto);

            var handler = new GetSubtitleByIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetSubtitleByIdQuery(Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<SubtitleDTO>(result.Value),
                () => Assert.True(result.Value.Id.Equals(Id)));
        }

        [Fact]
        public async Task Handler_Returns_NoMatching_Element()
        {
            // Arrange
            SetupRepository(new Subtitle());
            SetupMapper(new SubtitleDTO());

            var handler = new GetSubtitleByIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetSubtitleByIdQuery(Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.False(result.Value.Id.Equals(Id)));
        }

        private void SetupRepository(Subtitle returnElement)
        {
            _mockRepo.Setup(repo => repo.SubtitleRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Subtitle, bool>>>(),
                It.IsAny<Func<IQueryable<Subtitle>,
                IIncludableQueryable<Subtitle, object>>>()))
                .ReturnsAsync(returnElement);
        }

        private void SetupMapper(SubtitleDTO returnElement)
        {
            _mockMapper.Setup(x => x.Map<SubtitleDTO>(It.IsAny<object>()))
                .Returns(returnElement);
        }
    }
}
