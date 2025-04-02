using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using MockQueryable.Moq;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class GetTagsByStreetcodeIdHandlerTests
    {
        private const int StreetcodeId = 1;
        private const int IncorrectStreetcodeId = -1;
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly MockCannotFindLocalizer _mockLocalizer;

        private readonly List<StreetcodeTagIndex> _tags = new List<StreetcodeTagIndex>()
        {
            new StreetcodeTagIndex
            {
                Index = 1,
                IsVisible = true,
                Streetcode = new StreetcodeContent
                {
                    Id = StreetcodeId,
                },
                StreetcodeId = StreetcodeId,
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
                    Id = StreetcodeId,
                },
                StreetcodeId = StreetcodeId,
                Tag = new Tag()
                {
                    Id = 2,
                    Title = "title",
                },
            },
        };

        private readonly List<StreetcodeTagDTO> _tagDtOs = new List<StreetcodeTagDTO>()
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

        private readonly List<StreetcodeContent> _streetcodesUserHaveAccessTo = new List<StreetcodeContent>()
        {
            new StreetcodeContent
            {
                Id = StreetcodeId,
            },
        };

        public GetTagsByStreetcodeIdHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new MockCannotFindLocalizer();
        }

        [Fact]
        public async Task Handler_TagsForStreetcodeExists_ReturnsNotEmptyList()
        {
            // Arrange
            SetupRepository(_tags, _streetcodesUserHaveAccessTo);
            SetupMapper(_tagDtOs);

            var handler = new GetTagByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer);

            // Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(StreetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<StreetcodeTagDTO>>(result.Value),
                () => Assert.NotEmpty(result.Value));
        }

        [Fact]
        public async Task Handler_NoTagsForStreetcode_ReturnsEmptyList()
        {
            // Arrange
            SetupRepository(new List<StreetcodeTagIndex>(), _streetcodesUserHaveAccessTo);

            var handler = new GetTagByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer);

            // Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(StreetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<StreetcodeTagDTO>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<StreetcodeTagDTO>>(result.Value),
                () => Assert.Empty(result.Value));
        }

        [Fact]
        public async Task Handler_NoAccessToStreetcode_ReturnsErrorNoStreetcode()
        {
            // Arrange
            SetupRepository(_tags, new List<StreetcodeContent>());

            var expectedError = _mockLocalizer["CannotFindAnyStreetcodeWithCorrespondingId", StreetcodeId].Value;

            var handler = new GetTagByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer);

            // Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(StreetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private void SetupRepository(List<StreetcodeTagIndex> returnList, List<StreetcodeContent> streetcodeListUserCanAccess)
        {
            _mockRepo.Setup(repo => repo.StreetcodeTagIndexRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeTagIndex, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeTagIndex>,
                IIncludableQueryable<StreetcodeTagIndex, object>>>()))
                .ReturnsAsync(returnList);

            _mockRepo.Setup(repo => repo.StreetcodeRepository
                    .FindAll(
                        It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                        It.IsAny<Func<IQueryable<StreetcodeContent>,
                            IIncludableQueryable<StreetcodeContent, object>>>()))
                .Returns(streetcodeListUserCanAccess.AsQueryable().BuildMockDbSet().Object);
        }

        private void SetupMapper(List<StreetcodeTagDTO> returnList)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeTagDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }
    }
}
