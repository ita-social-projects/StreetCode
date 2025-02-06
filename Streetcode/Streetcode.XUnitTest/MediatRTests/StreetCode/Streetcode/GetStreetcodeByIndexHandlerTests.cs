using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using Model = Streetcode.DAL.Entities.Streetcode.StreetcodeContent;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetStreetcodeByIndexHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetStreetcodeByIndexHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsSuccess(int id)
        {
            // Arrange
            var testModelList = new List<Model>()
            {
                new (),
                new (),
                new (),
            };

            var testDtoList = new List<StreetcodeDTO>()
            {
                new PersonStreetcodeDTO(),
                new EventStreetcodeDTO(),
                new EventStreetcodeDTO(),
            };

            _repository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<Model, bool>>>(), null))
                .ReturnsAsync(testModelList);

            _mapper.Setup(x => x.Map<IEnumerable<StreetcodeDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(testDtoList);

            SetupLocalizers();

            var handler = new GetStreetcodeByIndexHandler(_repository.Object, _mapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetStreetcodeByIndexQuery(id), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsError(int id)
        {
            // Arrange
            var testStreetcodeDTO = new EventStreetcodeDTO();
            var expectedErrorMessage = $"Cannot find any streetcode with corresponding index: {id}";
            SetupLocalizers();

            Setup(null, testStreetcodeDTO);

            var handler = new GetStreetcodeByIndexHandler(_repository.Object, _mapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetStreetcodeByIndexQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsCorrectType(int id)
        {
            // Arrange
            var testStreetcodeDTO = new EventStreetcodeDTO();
            var testStreetcode = new Model();

            Setup(testStreetcode, testStreetcodeDTO);

            var handler = new GetStreetcodeByIndexHandler(_repository.Object, _mapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetStreetcodeByIndexQuery(id), CancellationToken.None);

            // Assert
            Assert.IsAssignableFrom<StreetcodeDTO>(result.Value);
        }

        private void Setup(Model? testStreetcode, EventStreetcodeDTO testStreetcodeDTO)
        {
            _repository
                .Setup(x => x.StreetcodeRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Model, bool>>?>(),
                        It.IsAny<Func<IQueryable<Model>,
                        IIncludableQueryable<Model, object>>>()))
                .ReturnsAsync(testStreetcode);

            _mapper
                .Setup(x => x.Map<StreetcodeDTO>(It.IsAny<object>()))
                .Returns(testStreetcodeDTO);
        }

        private void SetupLocalizers()
        {
            _mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
               .Returns((string key, object[] args) =>
               {
                   if (args != null && args.Length > 0 && args[0] is int id)
                   {
                       return new LocalizedString(key, $"Cannot find any streetcode with corresponding index: {id}");
                   }

                   return new LocalizedString(key, "Cannot find any streetcode with unknown index");
               });
        }
    }
}
