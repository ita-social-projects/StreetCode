using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Text.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetcodeTests.TextTests
{
    public class GetAllTextsTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;
        private readonly GetAllTextsHandler _handler;

        public GetAllTextsTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            _handler = new GetAllTextsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);
        }

        [Fact]
        public async Task ShouldReturnSuccesfullyAllTexts()
        {
            var testTextsList = new List<Text>()
            {
                new Text() { Id = 1 },
            };

            var testTextslistDto = new List<TextDTO>()
            {
                new TextDTO() { Id = 1 },
            };

            var testTexts = new Text() { Id = 1 };

            _mockRepository.Setup(repo => repo.TextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Text, bool>>>(), It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, Text>>?>()))
                .ReturnsAsync(testTexts);

            _mockRepository.Setup(repo => repo.TextRepository.GetAllAsync(null, null)).ReturnsAsync(testTextsList);

            _mockMapper
                .Setup(x => x.Map<IEnumerable<TextDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(testTextslistDto);
            _mockLocalizerCannotFind
                .Setup(x => x["CannotFindAnyText"])
                .Returns(new LocalizedString("CannotFindAnyText", "Cannot find any text"));

            var result = await _handler.Handle(new GetAllTextsQuery(UserRole.User), CancellationToken.None);

            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.NotEmpty(result.Value));
        }

        [Fact]
        public async Task GetAllTextsReturnError()
        {
            _mockRepository
                .Setup(repo => repo.TextRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Text, bool>>>(),
                        It.IsAny<Func<IQueryable<Text>,
                        IIncludableQueryable<Text, Text>>?>()))
                .ReturnsAsync((Text?)null);

            _mockRepository
                .Setup(repo => repo.TextRepository.GetAllAsync(It.IsAny<Expression<Func<Text, bool>>>(),
                        It.IsAny<Func<IQueryable<Text>,
                        IIncludableQueryable<Text, Text>>?>()))
                .ReturnsAsync(new List<Text>());

            _mockMapper
                .Setup(x => x.Map<IEnumerable<TextDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(new List<TextDTO>() { new TextDTO() { Id = 1 } });
            _mockLocalizerCannotFind
                .Setup(x => x["CannotFindAnyText"])
                .Returns(new LocalizedString("CannotFindAnyText", "Cannot find any text"));

            var result = await _handler.Handle(new GetAllTextsQuery(UserRole.User), CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}