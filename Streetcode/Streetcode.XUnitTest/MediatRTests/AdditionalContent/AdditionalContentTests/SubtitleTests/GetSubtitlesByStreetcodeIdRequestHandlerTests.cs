using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.SubtitleTests
{
    public class GetSubtitlesByStreetcodeIdRequestHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public GetSubtitlesByStreetcodeIdRequestHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        private const int _streetcode_id = 1;
        private const int _subtitle_id = 2;

        private readonly Subtitle subtitle = new Subtitle { Id = _subtitle_id, StreetcodeId = _streetcode_id };

		private readonly SubtitleDTO subtitleDTO = new SubtitleDTO { Id = _subtitle_id, StreetcodeId = _streetcode_id };
        async Task SetupRepository(Subtitle returnElement)
        {
			_mockRepo.Setup(repo => repo.SubtitleRepository.GetFirstOrDefaultAsync(
				It.IsAny<Expression<Func<Subtitle, bool>>>(),
				It.IsAny<Func<IQueryable<Subtitle>,
				IIncludableQueryable<Subtitle, object>>>()))
				.ReturnsAsync(returnElement);
		}
        async Task SetupMapper(SubtitleDTO returnElement)
        {
            _mockMapper.Setup(x => x.Map<SubtitleDTO>(It.IsAny<object>()))
                .Returns(returnElement);
		}

        [Fact]
        public async Task Handler_Returns_NotEmpty_Value()
        {
            //Arrange
            await SetupRepository(subtitle);
            await SetupMapper(subtitleDTO);

            var handler = new GetSubtitlesByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            //Act
            var result = await handler.Handle(new GetSubtitlesByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<SubtitleDTO>(result.Value),
                () => Assert.True(result.Value != null));
        }

        [Fact]
        public async Task Handler_Returns_corectValue()
		{
            //Arrange
			await SetupRepository(subtitle);
			await SetupMapper(subtitleDTO);

			var handler = new GetSubtitlesByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

			//Act
			var result = await handler.Handle(new GetSubtitlesByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

			//Assert
             Assert.True(result.Value.StreetcodeId == _streetcode_id);
		}
    }
}
