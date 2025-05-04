﻿using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.SubtitleTests
{
    public class GetSubtitlesByStreetcodeIdRequestHandlerTests
    {
        private const int streetcode_id = 1;
        private const int subtitle_id = 2;

        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;

        private readonly Subtitle subtitle = new Subtitle { Id = subtitle_id, StreetcodeId = streetcode_id };

        private readonly SubtitleDTO subtitleDTO = new SubtitleDTO { Id = subtitle_id, StreetcodeId = streetcode_id };

        public GetSubtitlesByStreetcodeIdRequestHandlerTests()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_Value()
        {
            // Arrange
            this.SetupRepository(this.subtitle);
            this.SetupMapper(this.subtitleDTO);

            var handler = new GetSubtitlesByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetSubtitlesByStreetcodeIdQuery(streetcode_id, UserRole.User), CancellationToken.None);

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

            var handler = new GetSubtitlesByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetSubtitlesByStreetcodeIdQuery(streetcode_id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.True(result.Value.StreetcodeId == streetcode_id);
        }

        private void SetupRepository(Subtitle returnElement)
        {
            this.mockRepo.Setup(repo => repo.SubtitleRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Subtitle, bool>>>(),
                It.IsAny<Func<IQueryable<Subtitle>,
                IIncludableQueryable<Subtitle, object>>>()))
                .ReturnsAsync(returnElement);
        }

        private void SetupMapper(SubtitleDTO returnElement)
        {
            this.mockMapper.Setup(x => x.Map<SubtitleDTO>(It.IsAny<object>()))
                .Returns(returnElement);
        }
    }
}