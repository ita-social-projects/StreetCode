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
using Streetcode.BLL.Interfaces.Logging;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetStreetcodeByIndexHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService> _mockLogger;
        public GetStreetcodeByIndexHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsSuccess(int id)
        {   
            // arrange
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

            _repository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<Model, bool>>>(), null))
                .ReturnsAsync(testModelList);

            _mapper.Setup(x => x.Map<IEnumerable<StreetcodeDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(testDTOList);

            var handler = new GetStreetcodeByIndexHandler(_repository.Object, _mapper.Object, _mockLogger.Object);
            // act
            var result = await handler.Handle(new GetStreetcodeByIndexQuery(id), CancellationToken.None);
            // assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsError(int id)
        {   
            // arrange 
            var testStreetcodeDTO = new EventStreetcodeDTO();
            var expectedErrorMessage = $"Cannot find any streetcode with corresponding index: {id}";

            Setup(null, testStreetcodeDTO);

            var handler = new GetStreetcodeByIndexHandler(_repository.Object, _mapper.Object, _mockLogger.Object);
            // act
            var result = await handler.Handle(new GetStreetcodeByIndexQuery(id), CancellationToken.None);
            // assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsCorrectType(int id)
        {   
            // arrange
            var testStreetcodeDTO = new EventStreetcodeDTO();
            var testStreetcode = new Model();

            Setup(testStreetcode, testStreetcodeDTO);

            var handler = new GetStreetcodeByIndexHandler(_repository.Object, _mapper.Object, _mockLogger.Object);
            // act
            var result = await handler.Handle(new GetStreetcodeByIndexQuery(id), CancellationToken.None);
            // assert
            Assert.IsAssignableFrom<StreetcodeDTO>(result.Value);
        }

        private void Setup(Model testStreetcode, EventStreetcodeDTO testStreetcodeDTO)
        {
            _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Model, bool>>?>(), It.IsAny<Func<IQueryable<Model>, IIncludableQueryable<Model, object>>>()))
                .ReturnsAsync(testStreetcode);

            _mapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<object>()))
                .Returns(testStreetcodeDTO);
        }

    }
}
