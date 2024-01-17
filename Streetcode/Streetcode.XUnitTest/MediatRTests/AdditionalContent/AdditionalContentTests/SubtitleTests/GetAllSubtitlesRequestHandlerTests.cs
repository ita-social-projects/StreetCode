﻿using System.Linq.Expressions;
using AutoMapper;
using Ical.Net.Serialization;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.SubtitleTests
{
    public class GetAllSubtitlesRequestHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public GetAllSubtitlesRequestHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        private readonly List<Subtitle> subtitles = new List<Subtitle>
        {
            new Subtitle
            {
                Id = 1,
                StreetcodeId = 1
            },
            new Subtitle
            {
                Id = 2,
                StreetcodeId = 1
            }
        };
        private readonly List<SubtitleDTO> subtitleDTOs = new List<SubtitleDTO> 
        {
            new SubtitleDTO
            {
                Id = 1,
                StreetcodeId = 1
            },
            new SubtitleDTO
            {
                Id = 2,
                StreetcodeId = 1
            }
        };

        async Task SetupRepository(List<Subtitle> returnList)
        {
            _mockRepo.Setup(repo => repo.SubtitleRepository.GetAllAsync(
                It.IsAny<Expression<Func<Subtitle, bool>>>(),
                It.IsAny<Func<IQueryable<Subtitle>,
                IIncludableQueryable<Subtitle, object>>>()))
                .ReturnsAsync(returnList);
        }

        async Task SetupMapper(List<SubtitleDTO> returnList)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<SubtitleDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            //Arrange
            await SetupRepository(subtitles);
            await SetupMapper(subtitleDTOs);

            var handler = new GetAllSubtitlesHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            //Act
            var result = await handler.Handle(new GetAllSubtitlesQuery(), CancellationToken.None);
            
            //Assert
            Assert.Multiple(
                () => Assert.IsType<List<SubtitleDTO>>(result.Value),
                () => Assert.True(result.Value.Count().Equals(subtitles.Count)));
        }

        [Fact]
        public async Task Handler_Returns_Empty_List()
        {
            //Arrange
            await SetupRepository(new List<Subtitle>());
            await SetupMapper(new List<SubtitleDTO>());

            var handler = new GetAllSubtitlesHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            //Act
            var result = await handler.Handle(new GetAllSubtitlesQuery(), CancellationToken.None);

            //Assert
            Assert.Multiple(
                    () => Assert.IsType<List<SubtitleDTO>>(result.Value),
                    () => Assert.Empty(result.Value));
        }
    }
}
