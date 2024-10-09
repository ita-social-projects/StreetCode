using AutoMapper;
using Moq;
using Org.BouncyCastle.Asn1.Ocsp;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Model = Streetcode.DAL.Entities.Streetcode.StreetcodeContent;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetAllStreetcodesHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService> _mockLogger;
        public GetAllStreetcodesHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task Handle_ReturnsSuccess()
        {
            // arrange
            var testModel = new Model();
            var testDTO = new PersonStreetcodeDTO();

            _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Model, bool>>>(), null))
                .ReturnsAsync(testModel);

            _mapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<object>()))
                .Returns(testDTO);

            var request = new GetAllStreetcodesRequestDTO();

            var handler = new GetAllStreetcodesHandler(_repository.Object, _mapper.Object);
            // act

            var result = await handler.Handle(new GetAllStreetcodesQuery(request), CancellationToken.None);

            // assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task Handle_ReturnsCorrectType()
        {
            // arrange
            var testModelList = new List<Model>();
            var testDTOList = new List<StreetcodeDTO>();

            _repository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<Model, bool>>>(), null))
                .ReturnsAsync(testModelList);

            _mapper.Setup(x => x.Map<IEnumerable<StreetcodeDTO>>(It.IsAny<IEnumerable<object>>()))
            .Returns(testDTOList);

            var handler = new GetAllStreetcodesHandler(_repository.Object, _mapper.Object);

            var request = new GetAllStreetcodesRequestDTO();

            // act
            var result = await handler.Handle(new GetAllStreetcodesQuery(request), CancellationToken.None);

            // assert
            Assert.IsAssignableFrom<GetAllStreetcodesResponseDTO>(result.Value);
        }

        [Fact]
        public async Task FindStreetCodeWithMatchTitleTest()
        {
            // Arrange
            var request = new GetAllStreetcodesRequestDTO
            {
                Title = "Some Title"
            };

            var query = new GetAllStreetcodesQuery(request);

            var (handler, repositoryWrapperMock) = SetupMockObjectsAndGetHandler(out var streetcodeRepositoryMock, out var mapperMock);

            IQueryable<StreetcodeContent> streetcodes = new List<StreetcodeContent>().AsQueryable();

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.NotNull(result.Value.Streetcodes);
        }

        [Fact]
        public async Task FindSortedStreetcodesTest()
        {
            // Arrange
            var request = new GetAllStreetcodesRequestDTO
            {
                Sort = "-Title"
            };

            var query = new GetAllStreetcodesQuery(request);

            var (handler, repositoryWrapperMock) = SetupMockObjectsAndGetHandler(out var streetcodeRepositoryMock, out var mapperMock);

            var streetcodes = new List<StreetcodeContent>
            {
                new StreetcodeContent { Id = 1, Title = "Streetcode 1" },
                new StreetcodeContent { Id = 2, Title = "Streetcode 2" },
                new StreetcodeContent { Id = 3, Title = "Streetcode 3" }
            }.AsQueryable();

            // Assert
            var result = await handler.Handle(query, default);
            var methodInfo = typeof(GetAllStreetcodesHandler).GetMethod("FindSortedStreetcodes", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { streetcodes, request.Sort };

            methodInfo.Invoke(handler, parameters);
            var sortedStreetcodes = (IQueryable<StreetcodeContent>)parameters[0];

            // Act
            var resultList = sortedStreetcodes.ToList();

            Assert.Equal("Streetcode 3", resultList[0].Title);
            Assert.Equal("Streetcode 2", resultList[1].Title);
            Assert.Equal("Streetcode 1", resultList[2].Title);
        }

        [Fact]
        public async Task FindFilteredStreetcodesTest()
        {
            // Arrange
            var request = new GetAllStreetcodesRequestDTO
            {
                Filter = "filter:Filter"
            };

            var query = new GetAllStreetcodesQuery(request);

            var (handler, repositoryWrapperMock) = SetupMockObjectsAndGetHandler(out var streetcodeRepositoryMock, out var mapperMock);
            var streetcodes = new List<StreetcodeContent>().AsQueryable();

            // Act
            var result = await handler.Handle(query, default);
            var findFilteredStreetcodesMethod = typeof(GetAllStreetcodesHandler).GetMethod("FindFilteredStreetcodes", BindingFlags.NonPublic | BindingFlags.Instance);
            findFilteredStreetcodesMethod.Invoke(handler, new object[] { streetcodes, request.Filter });

            // Assert
            Assert.Empty(streetcodes);
        }

        private (GetAllStreetcodesHandler handler, Mock<IRepositoryWrapper> repositoryWrapperMock) SetupMockObjectsAndGetHandler(out Mock<IStreetcodeRepository> streetcodeRepositoryMock, out Mock<IMapper> mapperMock)
        {
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            streetcodeRepositoryMock = new Mock<IStreetcodeRepository>();
            mapperMock = new Mock<IMapper>();

            repositoryWrapperMock.Setup(x => x.StreetcodeRepository).Returns(streetcodeRepositoryMock.Object);

            var handler = new GetAllStreetcodesHandler(repositoryWrapperMock.Object, mapperMock.Object);

            return (handler, repositoryWrapperMock);
        }
    }
}
