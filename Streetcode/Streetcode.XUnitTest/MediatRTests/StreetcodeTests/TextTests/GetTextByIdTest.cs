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
    public class GetTextByIdTest
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetTextByIdTest()
        {
            this._repository = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(2)]
        public async Task GetTextByIdSuccess(int id)
        {
            var testText = new Text() { Id = id };

            this._repository
                .Setup(repo => repo.TextRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Text, bool>>>(),
                        It.IsAny<Func<IQueryable<Text>,
                        IIncludableQueryable<Text, Text>>?>()))
                .ReturnsAsync(testText);

            this._mockMapper.Setup(x => x.Map<TextDTO>(It.IsAny<Text>())).Returns((Text sourceText) =>
            {
                return new TextDTO { Id = sourceText.Id };
            });
            var handler = new GetTextByIdHandler(this._repository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            var result = await handler.Handle(new GetTextByIdQuery(id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(id, result.Value.Id);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetTextByIdTypeCheck(int id)
        {
            var testText = new Text() { Id = id };

            this._repository
                .Setup(repo => repo.TextRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Text, bool>>>(),
                        It.IsAny<Func<IQueryable<Text>,
                        IIncludableQueryable<Text, Text>>?>()))
                .ReturnsAsync(testText);

            this._mockMapper.Setup(x => x.Map<TextDTO>(It.IsAny<Text>())).Returns((Text sourceText) =>
            {
                return new TextDTO { Id = sourceText.Id };
            });

            var handler = new GetTextByIdHandler(this._repository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            var result = await handler.Handle(new GetTextByIdQuery(2), CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<TextDTO>(result.Value);
        }
    }
}