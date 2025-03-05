using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.MediatR.Streetcode.Text.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.StreetcodeTest.TextTest
{
    public class GetAllTextsTest
    {
        private readonly Mock<IRepositoryWrapper> repository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public GetAllTextsTest()
        {
            this.repository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
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

            this.repository.Setup(repo => repo.TextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Text, bool>>>(), It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, Text>>?>()))
                .ReturnsAsync(testTexts);

            this.repository.Setup(repo => repo.TextRepository.GetAllAsync(null, null)).ReturnsAsync(testTextsList);

            this.mockMapper
                .Setup(x => x.Map<IEnumerable<TextDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(testTextslistDTO);
            this.mockLocalizerCannotFind
                .Setup(x => x["CannotFindAnyText"])
                .Returns(new LocalizedString("CannotFindAnyText", "Cannot find any text"));

            var handler = new GetAllTextsHandler(this.repository.Object, this.mockMapper.Object);

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

            this.mockMapper
                .Setup(x => x.Map<IEnumerable<TextDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(new List<TextDTO>() { new TextDTO() { Id = 1 } });
            this.mockLocalizerCannotFind
                .Setup(x => x["CannotFindAnyText"])
                .Returns(new LocalizedString("CannotFindAnyText", "Cannot find any text"));

            var handler = new GetAllTextsHandler(repository.Object, this.mockMapper.Object);

            var result = await handler.Handle(new GetAllTextsQuery(), CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
