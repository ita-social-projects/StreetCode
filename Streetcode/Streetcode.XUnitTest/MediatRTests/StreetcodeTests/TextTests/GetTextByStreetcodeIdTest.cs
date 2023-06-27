using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.MediatR.Streetcode.Text.GetById;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.StreetcodeTest.TextTest
{
  public class GetTextByStreetcodeIdTest
    {
        private Mock<IRepositoryWrapper> repository;
        private Mock<IMapper> mockMapper;

        public GetTextByStreetcodeIdTest()
        {
            repository = new Mock<IRepositoryWrapper>();
            mockMapper = new Mock<IMapper>();
        }

        [Theory]
        [InlineData(1)]
        public async Task GetTextByStreetcodeIdSuccess(int id)
        {
            var testText = new Text() { StreetcodeId = id };

            repository.Setup(repo => repo.TextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Text, bool>>>(),
                It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, Text>>?>()))
            .ReturnsAsync(testText);

            mockMapper.Setup(x => x.Map<TextDTO>(It.IsAny<Text>())).Returns((Text sourceText) =>
            {
                return new TextDTO { StreetcodeId = sourceText.StreetcodeId };
            });
            var handler = new GetTextByIdHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetTextByIdQuery(id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(id, result.Value.StreetcodeId);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetTextByStreetcodeIdTypeCheck(int id)
        {
            var testText = new Text() { StreetcodeId = id };

            repository.Setup(repo => repo.TextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Text, bool>>>(),
                It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, Text>>?>()))
            .ReturnsAsync(testText);

            mockMapper.Setup(x => x.Map<TextDTO>(It.IsAny<Text>())).Returns((Text sourceText) =>
            {
                return new TextDTO { StreetcodeId = sourceText.StreetcodeId };
            });

            var handler = new GetTextByIdHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetTextByIdQuery(id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<TextDTO>(result.ValueOrDefault);
        }
    }
}