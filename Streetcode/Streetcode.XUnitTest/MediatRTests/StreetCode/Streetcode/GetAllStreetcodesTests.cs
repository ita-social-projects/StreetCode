using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

using Model = Streetcode.DAL.Entities.Streetcode.StreetcodeContent;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetAllStreetcodesTests
    {
        [Fact]
        public async Task GetAllStreetcodes_Success()
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testModel = new Model();
            var testDTO = new PersonStreetcodeDTO();

            repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Model, bool>>>(), null))
                .ReturnsAsync(testModel);

            mockMapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<object>()))
                .Returns(testDTO);

            var handler = new GetAllStreetcodesHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetAllStreetcodesQuery(), CancellationToken.None);

            Assert.NotNull(result);

        }
        [Fact]
        public async Task GetAllStreetcodes_TypeCheck()
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testModelList = new List<Model>();
            var testDTOList = new List<StreetcodeDTO>();

            repository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<Model, bool>>>(), null))
                .ReturnsAsync(testModelList);

            mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeDTO>>(It.IsAny<IEnumerable<object>>()))
            .Returns(testDTOList);

            var handler = new GetAllStreetcodesHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetAllStreetcodesQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<StreetcodeDTO>>(result.Value);
        }
    }
}
