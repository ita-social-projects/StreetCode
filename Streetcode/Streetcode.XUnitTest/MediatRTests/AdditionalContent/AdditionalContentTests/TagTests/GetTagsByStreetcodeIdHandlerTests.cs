using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
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
        private const int _streetcode_id = 1;
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        private readonly List<StreetcodeTagIndex> tags = new List<StreetcodeTagIndex>()
        {
            new StreetcodeTagIndex
            {
                Index = 1,
                IsVisible = true,
                Streetcode = new StreetcodeContent
                {
                    Id = _streetcode_id,
                },
                StreetcodeId = _streetcode_id,
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
                    Id = _streetcode_id,
                },
                StreetcodeId = _streetcode_id,
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
            this._mockRepo = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            // Arrange
            this.SetupRepository(this.tags);
            this.SetupMapper(this.tagDTOs);

            var handler = new GetTagByStreetcodeIdHandler(this._mockRepo.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<StreetcodeTagDTO>>(result.Value),
                () => Assert.NotEmpty(result.Value));
        }

        [Fact]
        public async Task Handler_Returns_Error()
        {
            // Arrange
            this.SetupRepository(new List<StreetcodeTagIndex>());
            this.SetupMapper(new List<StreetcodeTagDTO>());

            var expectedError = $"Cannot find any tag by the streetcode id: {_streetcode_id}";
            this._mockLocalizer.Setup(localizer => localizer["CannotFindAnyTagByTheStreetcodeId", _streetcode_id])
                .Returns(new LocalizedString("CannotFindAnyTagByTheStreetcodeId", expectedError));

            var handler = new GetTagByStreetcodeIdHandler(this._mockRepo.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private void SetupRepository(List<StreetcodeTagIndex> returnList)
        {
            this._mockRepo.Setup(repo => repo.StreetcodeTagIndexRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeTagIndex, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeTagIndex>,
                IIncludableQueryable<StreetcodeTagIndex, object>>>()))
                .ReturnsAsync(returnList);
        }

        private void SetupMapper(List<StreetcodeTagDTO> returnList)
        {
            this._mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeTagDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }
    }
}
