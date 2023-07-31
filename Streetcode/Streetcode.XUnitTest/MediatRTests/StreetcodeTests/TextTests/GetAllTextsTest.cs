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
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.StreetcodeTest.TextTest
{
  public class GetAllTextsTest
    {
        private readonly Mock<IRepositoryWrapper> repository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetAllTextsTest()
        {
            repository = new Mock<IRepositoryWrapper>();
            mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccesfullyAllTexts()
        {

            var testTextsList = new List<Text>()
        {
            new Text() { Id = 1 }
                   };

            var testTextslistDTO = new List<TextDTO>()
        {
            new TextDTO() { Id = 1 }
        };

            var testTexts = new Text() { Id = 1 };

            repository.Setup(repo => repo.TextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Text, bool>>>(), It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, Text>>?>()))
                .ReturnsAsync(testTexts);

            repository.Setup(repo => repo.TextRepository.GetAllAsync(null, null)).ReturnsAsync(testTextsList);

            mockMapper.Setup(x => x.Map<IEnumerable<TextDTO>>(It.IsAny<IEnumerable<object>>())).Returns(testTextslistDTO);
            _mockLocalizerCannotFind.Setup(x => x["CannotFindAnyText"])
               .Returns(new LocalizedString("CannotFindAnyText", "Cannot find any text"));

            var handler = new GetAllTextsHandler(repository.Object, mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            var result = await handler.Handle(new GetAllTextsQuery(), CancellationToken.None);

            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.NotEmpty(result.Value));
        }

        [Fact]
        public async Task GetAllTextsReturnError()
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();


                repository.Setup(repo => repo.TextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Text, bool>>>(),
                It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, Text>>?>()))
                .ReturnsAsync((Text)null);

            repository.Setup(repo => repo.TextRepository.GetAllAsync(null, null)).ReturnsAsync((List<Text>)null);

            mockMapper.Setup(x => x.Map<IEnumerable<TextDTO>>(It.IsAny<IEnumerable<object>>())).Returns(new List<TextDTO>() { new TextDTO() { Id = 1 } });
            _mockLocalizerCannotFind.Setup(x => x["CannotFindAnyText"])
               .Returns(new LocalizedString("CannotFindAnyText", "Cannot find any text"));


            var handler = new GetAllTextsHandler(repository.Object, mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            var result = await handler.Handle(new GetAllTextsQuery(), CancellationToken.None);

            Assert.NotNull(result);

        }
    }

}
