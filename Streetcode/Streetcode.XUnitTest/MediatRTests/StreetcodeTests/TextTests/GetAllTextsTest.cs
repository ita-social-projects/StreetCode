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
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.StreetcodeTest.TextTest
{
    public class GetAllTextsTest
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetAllTextsTest()
        {
            this._repository = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccesfullyAllTexts()
        {
            var testTextsList = new List<Text>()
            {
                new Text() { Id = 1 },
            };

            var testTextslistDTO = new List<TextDTO>()
            {
                new TextDTO() { Id = 1 },
            };

            var testTexts = new Text() { Id = 1 };

            this._repository.Setup(repo => repo.TextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Text, bool>>>(), It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, Text>>?>()))
                .ReturnsAsync(testTexts);

            this._repository.Setup(repo => repo.TextRepository.GetAllAsync(null, null)).ReturnsAsync(testTextsList);

            this._mockMapper
                .Setup(x => x.Map<IEnumerable<TextDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(testTextslistDTO);
            this._mockLocalizerCannotFind
                .Setup(x => x["CannotFindAnyText"])
                .Returns(new LocalizedString("CannotFindAnyText", "Cannot find any text"));

            var handler = new GetAllTextsHandler(this._repository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            var result = await handler.Handle(new GetAllTextsQuery(), CancellationToken.None);

            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.NotEmpty(result.Value));
        }

        [Fact]
        public async Task GetAllTextsReturnError()
        {
            var repository = new Mock<IRepositoryWrapper>();

            repository
                .Setup(repo => repo.TextRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Text, bool>>>(),
                        It.IsAny<Func<IQueryable<Text>,
                        IIncludableQueryable<Text, Text>>?>()))
                .ReturnsAsync((Text?)null);

            repository
                .Setup(repo => repo.TextRepository.GetAllAsync(null, null))
                .ReturnsAsync(new List<Text>());

            this._mockMapper
                .Setup(x => x.Map<IEnumerable<TextDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(new List<TextDTO>() { new TextDTO() { Id = 1 } });
            this._mockLocalizerCannotFind
                .Setup(x => x["CannotFindAnyText"])
                .Returns(new LocalizedString("CannotFindAnyText", "Cannot find any text"));

            var handler = new GetAllTextsHandler(repository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            var result = await handler.Handle(new GetAllTextsQuery(), CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
