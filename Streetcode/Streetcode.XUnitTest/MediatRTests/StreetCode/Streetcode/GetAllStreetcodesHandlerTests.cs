using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;
using Xunit;
using Model = Streetcode.DAL.Entities.Streetcode.StreetcodeContent;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetAllStreetcodesHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> repository;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<ILoggerService> mockLogger;

        public GetAllStreetcodesHandlerTests()
        {
            this.repository = new Mock<IRepositoryWrapper>();
            this.mapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task Handle_ReturnsSuccess()
        {
            // Arrange
            var testModel = new Model();
            var testDTO = new PersonStreetcodeDTO();

            this.repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Model, bool>>>(), null))
                .ReturnsAsync(testModel);

            this.mapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<object>()))
                .Returns(testDTO);

            var request = new GetAllStreetcodesRequestDTO();

            var handler = new GetAllStreetcodesHandler(this.repository.Object, this.mapper.Object);

            // Act
            var result = await handler.Handle(new GetAllStreetcodesQuery(request), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Handle_ReturnsCorrectType()
        {
            // Arrange
            var testModelList = new List<Model>();
            var testDTOList = new List<StreetcodeDTO>();

            this.repository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<Model, bool>>>(), null))
                .ReturnsAsync(testModelList);

            this.mapper.Setup(x => x.Map<IEnumerable<StreetcodeDTO>>(It.IsAny<IEnumerable<object>>()))
            .Returns(testDTOList);

            var handler = new GetAllStreetcodesHandler(this.repository.Object, this.mapper.Object);

            var request = new GetAllStreetcodesRequestDTO();

            // Act
            var result = await handler.Handle(new GetAllStreetcodesQuery(request), CancellationToken.None);

            // Assert
            Assert.IsAssignableFrom<GetAllStreetcodesResponseDTO>(result.Value);
        }

        [Fact]
        public async Task FindStreetCodeWithMatchTitleTest()
        {
            // Arrange
            var request = new GetAllStreetcodesRequestDTO
            {
                Title = "Some Title",
            };

            var query = new GetAllStreetcodesQuery(request);

            var (handler, _) = this.SetupMockObjectsAndGetHandler(out var streetcodeRepositoryMock, out var mapperMock);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.NotNull(result.Value.Streetcodes);
        }

        [Fact]
        public void FindSortedStreetcodesTest()
        {
            // Arrange
            var request = new GetAllStreetcodesRequestDTO
            {
                Sort = "-Title",
            };

            var (handler, _) = this.SetupMockObjectsAndGetHandler(out var streetcodeRepositoryMock, out var mapperMock);

            var streetcodes = new List<StreetcodeContent>
            {
                new StreetcodeContent { Id = 1, Title = "Streetcode 1" },
                new StreetcodeContent { Id = 2, Title = "Streetcode 2" },
                new StreetcodeContent { Id = 3, Title = "Streetcode 3" },
            }.AsQueryable();

            // Assert
            var methodInfo = typeof(GetAllStreetcodesHandler).GetMethod("FindSortedStreetcodes", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { streetcodes, request.Sort };

            methodInfo?.Invoke(handler, parameters);
            var sortedStreetcodes = (IQueryable<StreetcodeContent>)parameters[0];

            // Act
            var resultList = sortedStreetcodes.ToList();

            Assert.Equal("Streetcode 3", resultList[0].Title);
            Assert.Equal("Streetcode 2", resultList[1].Title);
            Assert.Equal("Streetcode 1", resultList[2].Title);
        }

        [Fact]
        public void FindFilteredStreetcodesTest()
        {
            // Arrange
            var request = new GetAllStreetcodesRequestDTO
            {
                Filter = "filter:Filter",
            };

            var (handler, _) = this.SetupMockObjectsAndGetHandler(out var streetcodeRepositoryMock, out var mapperMock);
            var streetcodes = new List<StreetcodeContent>().AsQueryable();

            // Act
            var findFilteredStreetcodesMethod = typeof(GetAllStreetcodesHandler).GetMethod("FindFilteredStreetcodes", BindingFlags.NonPublic | BindingFlags.Instance);
            findFilteredStreetcodesMethod?.Invoke(handler, new object[] { streetcodes, request.Filter });

            // Assert
            Assert.Empty(streetcodes);
        }

        private (GetAllStreetcodesHandler handler, Mock<IRepositoryWrapper> repositoryWrapperMock) SetupMockObjectsAndGetHandler(out Mock<IStreetcodeRepository> streetcodeRepositoryMock, out Mock<IMapper> mapperMock)
        {
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            streetcodeRepositoryMock = new Mock<IStreetcodeRepository>();
            mapperMock = new Mock<IMapper>();

            repositoryWrapperMock
                .Setup(x => x.StreetcodeRepository)
                .Returns(streetcodeRepositoryMock.Object);

            var handler = new GetAllStreetcodesHandler(repositoryWrapperMock.Object, mapperMock.Object);

            return (handler, repositoryWrapperMock);
        }
    }
}
