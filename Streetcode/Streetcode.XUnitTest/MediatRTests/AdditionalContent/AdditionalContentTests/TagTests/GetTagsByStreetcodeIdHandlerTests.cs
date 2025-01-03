using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class GetTagsByStreetcodeIdHandlerTests
    {
        private const int streetcode_id = 1;
        private const int incorrect_streetcode_id = -1;
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;

        private readonly List<StreetcodeTagIndex> tags = new List<StreetcodeTagIndex>()
        {
            new StreetcodeTagIndex
            {
                Index = 1,
                IsVisible = true,
                Streetcode = new StreetcodeContent
                {
                    Id = streetcode_id,
                },
                StreetcodeId = streetcode_id,
                Tag = new Tag()
                {
                    Id = 1,
                    Title = "title",
                },
            },
            new StreetcodeTagIndex
            {
                Index = 2,
                IsVisible = true,
                Streetcode = new StreetcodeContent
                {
                    Id = streetcode_id,
                },
                StreetcodeId = streetcode_id,
                Tag = new Tag()
                {
                    Id = 2,
                    Title = "title",
                },
            },
        };

        private readonly List<StreetcodeTagDTO> tagDTOs = new List<StreetcodeTagDTO>()
        {
            new StreetcodeTagDTO
            {
                Id = 1,
                Title = "title",
                IsVisible = true,
                Index = 1,
            },
            new StreetcodeTagDTO
            {
                Id = 2,
                Title = "title",
                IsVisible = true,
                Index = 2,
            },
        };

        public GetTagsByStreetcodeIdHandlerTests()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            // Arrange
            this.SetupRepository(this.tags);
            this.SetupMapper(this.tagDTOs);

            var handler = new GetTagByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(streetcode_id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<StreetcodeTagDTO>>(result.Value),
                () => Assert.NotEmpty(result.Value));
        }

        [Fact]
        public async Task Handler_Returns_Empty_List()
        {
            // Arrange
            this.SetupRepository(new List<StreetcodeTagIndex>());

            var handler = new GetTagByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(streetcode_id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<StreetcodeTagDTO>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<StreetcodeTagDTO>>(result.Value),
                () => Assert.Empty(result.Value));
        }

        [Fact]
        public async Task Handler_Returns_Error()
        {
            // Arrange
             var expectedError = $"Cannot find any tag by the streetcode id: {streetcode_id}";
            this.mockLocalizer.Setup(localizer => localizer["CannotFindAnyTagByTheStreetcodeId", streetcode_id])
                .Returns(new LocalizedString("CannotFindAnyTagByTheStreetcodeId", expectedError));
            
            var handler = new GetTagByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(incorrect_streetcode_id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private void SetupRepository(List<StreetcodeTagIndex> returnList)
        {
            this.mockRepo.Setup(repo => repo.StreetcodeTagIndexRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeTagIndex, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeTagIndex>,
                IIncludableQueryable<StreetcodeTagIndex, object>>>()))
                .ReturnsAsync(returnList);
        }

        private void SetupMapper(List<StreetcodeTagDTO> returnList)
        {
            this.mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeTagDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }
    }
}
