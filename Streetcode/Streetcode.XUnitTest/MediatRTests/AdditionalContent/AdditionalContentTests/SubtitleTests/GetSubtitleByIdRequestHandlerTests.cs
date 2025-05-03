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
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.AdditionalContentTests.SubtitleTests
{
    public class GetSubtitleByIdRequestHandlerTests
    {
        private const int id = 1;
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;

        private readonly Subtitle subtitle = new Subtitle { Id = id };
        private readonly SubtitleDTO subtitleDTO = new SubtitleDTO { Id = id };

        public GetSubtitleByIdRequestHandlerTests()
        {
            mockRepo = new Mock<IRepositoryWrapper>();
            mockMapper = new Mock<IMapper>();
            mockLogger = new Mock<ILoggerService>();
            mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handler_Returns_Matching_Element()
        {
            // Arrange
            SetupRepository(subtitle);
            SetupMapper(subtitleDTO);

            var handler = new GetSubtitleByIdHandler(mockRepo.Object, mockMapper.Object, mockLogger.Object, mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetSubtitleByIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<SubtitleDTO>(result.Value),
                () => Assert.True(result.Value.Id.Equals(id)));
        }

        [Fact]
        public async Task Handler_Returns_NoMatching_Element()
        {
            // Arrange
            SetupRepository(new Subtitle());
            SetupMapper(new SubtitleDTO());

            var handler = new GetSubtitleByIdHandler(mockRepo.Object, mockMapper.Object, mockLogger.Object, mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetSubtitleByIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.False(result.Value.Id.Equals(id)));
        }

        private void SetupRepository(Subtitle returnElement)
        {
            mockRepo.Setup(repo => repo.SubtitleRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Subtitle, bool>>>(),
                It.IsAny<Func<IQueryable<Subtitle>,
                IIncludableQueryable<Subtitle, object>>>()))
                .ReturnsAsync(returnElement);
        }

        private void SetupMapper(SubtitleDTO returnElement)
        {
            mockMapper.Setup(x => x.Map<SubtitleDTO>(It.IsAny<object>()))
                .Returns(returnElement);
        }
    }
}
