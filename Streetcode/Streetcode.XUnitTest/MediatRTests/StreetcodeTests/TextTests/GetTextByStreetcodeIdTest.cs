using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Text.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.StreetcodeTest.TextTest
{
    public class GetTextByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> repository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public GetTextByStreetcodeIdTest()
        {
            this.repository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task GetTextByStreetcodeIdSuccess(int id)
        {
            var testText = new Text() { StreetcodeId = id };

            this.repository
                .Setup(repo => repo.TextRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Text, bool>>>(),
                        It.IsAny<Func<IQueryable<Text>,
                        IIncludableQueryable<Text, Text>>?>()))
                .ReturnsAsync(testText);

            this.mockMapper.Setup(x => x.Map<TextDTO>(It.IsAny<Text>())).Returns((Text sourceText) =>
            {
                return new TextDTO { StreetcodeId = sourceText.StreetcodeId };
            });
            var handler = new GetTextByIdHandler(this.repository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            var result = await handler.Handle(new GetTextByIdQuery(id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(id, result.Value.StreetcodeId);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetTextByStreetcodeIdTypeCheck(int id)
        {
            var testText = new Text() { StreetcodeId = id };

            this.repository
                .Setup(repo => repo.TextRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Text, bool>>>(),
                        It.IsAny<Func<IQueryable<Text>,
                        IIncludableQueryable<Text, Text>>?>()))
            .ReturnsAsync(testText);

            this.mockMapper.Setup(x => x.Map<TextDTO>(It.IsAny<Text>())).Returns((Text sourceText) =>
            {
                return new TextDTO { StreetcodeId = sourceText.StreetcodeId };
            });

            var handler = new GetTextByIdHandler(this.repository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            var result = await handler.Handle(new GetTextByIdQuery(id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<TextDTO>(result.ValueOrDefault);
        }
    }
}