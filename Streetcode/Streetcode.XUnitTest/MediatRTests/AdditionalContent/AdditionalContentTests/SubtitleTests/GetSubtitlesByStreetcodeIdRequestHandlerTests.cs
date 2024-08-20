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
        private const int _streetcode_id = 1;
        private const int _subtitle_id = 2;

        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        private readonly Subtitle subtitle = new Subtitle { Id = _subtitle_id, StreetcodeId = _streetcode_id };

        private readonly SubtitleDTO subtitleDTO = new SubtitleDTO { Id = _subtitle_id, StreetcodeId = _streetcode_id };

        public GetSubtitlesByStreetcodeIdRequestHandlerTests()
        {
            this._mockRepo = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_Value()
        {
            // Arrange
            this.SetupRepository(this.subtitle);
            this.SetupMapper(this.subtitleDTO);

            var handler = new GetSubtitlesByStreetcodeIdHandler(this._mockRepo.Object, this._mockMapper.Object, this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetSubtitlesByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<SubtitleDTO>(result.Value),
                () => Assert.True(result.Value != null));
        }

        [Fact]
        public async Task Handler_Returns_corectValue()
        {
            // Arrange
            this.SetupRepository(this.subtitle);
            this.SetupMapper(this.subtitleDTO);

            var handler = new GetSubtitlesByStreetcodeIdHandler(this._mockRepo.Object, this._mockMapper.Object, this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetSubtitlesByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            // Assert
            Assert.True(result.Value.StreetcodeId == _streetcode_id);
        }

        private void SetupRepository(Subtitle returnElement)
        {
            this._mockRepo.Setup(repo => repo.SubtitleRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Subtitle, bool>>>(),
                It.IsAny<Func<IQueryable<Subtitle>,
                IIncludableQueryable<Subtitle, object>>>()))
                .ReturnsAsync(returnElement);
        }

        private void SetupMapper(SubtitleDTO returnElement)
        {
            this._mockMapper.Setup(x => x.Map<SubtitleDTO>(It.IsAny<object>()))
                .Returns(returnElement);
        }
    }
}
