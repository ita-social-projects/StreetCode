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
    public class GetAllStreetcodesHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        public GetAllStreetcodesHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }
        [Fact]
        public async Task Handle_ReturnsSuccess()
        {
            var testModel = new Model();
            var testDTO = new PersonStreetcodeDTO();

            _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Model, bool>>>(), null))
                .ReturnsAsync(testModel);

            _mapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<object>()))
                .Returns(testDTO);

            var handler = new GetAllStreetcodesHandler(_repository.Object, _mapper.Object);

            var result = await handler.Handle(new GetAllStreetcodesQuery(), CancellationToken.None);

            Assert.NotNull(result);

        }
        [Fact]
        public async Task Handle_ReturnsCorrectType()
        {
            var testModelList = new List<Model>();
            var testDTOList = new List<StreetcodeDTO>();

            _repository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<Model, bool>>>(), null))
                .ReturnsAsync(testModelList);

            _mapper.Setup(x => x.Map<IEnumerable<StreetcodeDTO>>(It.IsAny<IEnumerable<object>>()))
            .Returns(testDTOList);

            var handler = new GetAllStreetcodesHandler(_repository.Object, _mapper.Object);

            var result = await handler.Handle(new GetAllStreetcodesQuery(), CancellationToken.None);

            Assert.IsAssignableFrom<IEnumerable<StreetcodeDTO>>(result.Value);
        }
    }
}
