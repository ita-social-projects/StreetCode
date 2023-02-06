using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using System.Linq.Expressions;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex;
using Streetcode.DAL.Entities.Streetcode;
using Microsoft.EntityFrameworkCore.Query;

using Model = Streetcode.DAL.Entities.Streetcode.StreetcodeContent;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetStreetcodeByIndexTests
    {
        [Theory]
        [InlineData(1)]
        public async Task GetStreetcodeByIndex_Success(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testModelList = new List<Model>()
            {
                new Model(),
                new Model(),
                new Model()
            };

            var testDTOList = new List<StreetcodeDTO>()
            {
                new PersonStreetcodeDTO(),
                new EventStreetcodeDTO(),
                new EventStreetcodeDTO()
            };

            repository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<Model, bool>>>(), null))
                .ReturnsAsync(testModelList);

            mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(testDTOList);

            var handler = new GetStreetcodeByIndexHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetStreetcodeByIndexQuery(id), CancellationToken.None);

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetStreetcodeByIndex_Error(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testStreetcodeDTO = new EventStreetcodeDTO();

            repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync((StreetcodeContent)null);

            mockMapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<object>()))
            .Returns(testStreetcodeDTO);

            var handler = new GetStreetcodeByIndexHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetStreetcodeByIndexQuery(id), CancellationToken.None);

            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(result.Errors.First().Message, $"Cannot find a streetcode with corresponding Index: {id}");
        }

        [Theory]
        [InlineData(1)]
        public async Task GetStreetcodeByIndex_TypeCheck(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testStreetcodeDTO = new EventStreetcodeDTO();
            var testStreetcode = new StreetcodeContent();

            repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(testStreetcode);

            mockMapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<object>()))
                .Returns(testStreetcodeDTO);

            var handler = new GetStreetcodeByIndexHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetStreetcodeByIndexQuery(id), CancellationToken.None);

            Assert.IsAssignableFrom<StreetcodeDTO>(result.Value);
        }

    }
}
