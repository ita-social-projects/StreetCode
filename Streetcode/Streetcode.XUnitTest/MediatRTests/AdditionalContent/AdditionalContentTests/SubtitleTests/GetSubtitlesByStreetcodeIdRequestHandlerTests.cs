using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.SubtitleTests
{
    public class GetSubtitlesByStreetcodeIdRequestHandlerTests
    {
        private const int StreetcodeId = 1;
        private const int SubtitleId = 2;

        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        private readonly Subtitle _subtitle = new Subtitle { Id = SubtitleId, StreetcodeId = StreetcodeId };

        private readonly SubtitleDTO _subtitleDto = new SubtitleDTO { Id = SubtitleId, StreetcodeId = StreetcodeId };

        public GetSubtitlesByStreetcodeIdRequestHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_Value()
        {
            // Arrange
            SetupRepository(_subtitle);
            SetupMapper(_subtitleDto);

            var handler = new GetSubtitlesByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetSubtitlesByStreetcodeIdQuery(StreetcodeId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<SubtitleDTO>(result.Value),
                () => Assert.True(result.Value != null));
        }

        [Fact]
        public async Task Handler_Returns_corectValue()
        {
            // Arrange
            SetupRepository(_subtitle);
            SetupMapper(_subtitleDto);

            var handler = new GetSubtitlesByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetSubtitlesByStreetcodeIdQuery(StreetcodeId), CancellationToken.None);

            // Assert
            Assert.True(result.Value.StreetcodeId == StreetcodeId);
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
