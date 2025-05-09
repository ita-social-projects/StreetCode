using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.DAL.Enums;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllShort;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Helpers;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetAllStreetcodesShortHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly MockNoSharedResourceLocalizer _mockNoSharedResourceLocalizer;
        private readonly GetAllStreetcodesShortHandler _handler;

        public GetAllStreetcodesShortHandlerTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockNoSharedResourceLocalizer = new MockNoSharedResourceLocalizer();
            _handler = new GetAllStreetcodesShortHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockNoSharedResourceLocalizer);
        }

        [Fact]
        public async Task ShouldGetAllSuccessfully_WhenStreetcodesExist()
        {
            // Arrange
            const int objectsNumber = 2;
            var (streetcodeContentsPaginated, streetcodeShortDtoList) = GetStreetcodeObjects(objectsNumber);
            var request = GetRequest(UserRole.User);

            MockHelpers.SetupMockStreetcodeRepositoryGetAllPaginated(_mockRepository, streetcodeContentsPaginated);
            MockHelpers.SetupMockMapper<IEnumerable<StreetcodeShortDto>, IEnumerable<StreetcodeContent>>(
                _mockMapper,
                streetcodeShortDtoList,
                streetcodeContentsPaginated.Entities);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.StreetcodesShort.Should().SatisfyRespectively(
                first => first.Id.Should().Be(streetcodeContentsPaginated.Entities.ToList()[0].Id),
                second => second.Id.Should().Be(streetcodeContentsPaginated.Entities.ToList()[1].Id));
            result.Value.TotalAmount.Should().Be(objectsNumber);
        }

        [Fact]
        public async Task ShouldGetAllSuccessfully_WithCorrectDataType()
        {
            // Arrange
            const int objectsNumber = 2;
            var (termsPaginated, termDtoList) = GetStreetcodeObjects(objectsNumber);
            var request = GetRequest(UserRole.User);

            MockHelpers.SetupMockStreetcodeRepositoryGetAllPaginated(_mockRepository, termsPaginated);
            MockHelpers.SetupMockMapper<IEnumerable<StreetcodeShortDto>, IEnumerable<StreetcodeContent>>(
                _mockMapper,
                termDtoList,
                termsPaginated.Entities);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<GetAllStreetcodesShortDto>();
        }

        [Fact]
        public async Task ShouldGetAllFailingly_WhenStreetcodesNotExist()
        {
            // Arrange
            var (emptyTermsPaginated, emptyTermDtoList) = GetEmptyStreetcodeObjects();
            var request = GetRequest(UserRole.User);
            var expectedErrorMessage = _mockNoSharedResourceLocalizer["NoStreetcodesExistNow"].Value;

            MockHelpers.SetupMockStreetcodeRepositoryGetAllPaginated(_mockRepository, emptyTermsPaginated);
            MockHelpers.SetupMockMapper(_mockMapper, emptyTermDtoList, emptyTermsPaginated.Entities);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Be(expectedErrorMessage);
            _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
        }

        [Fact]
        public async Task ShouldGetAllSuccessfully_WithCorrectPageSize()
        {
            // Arrange
            const ushort pageNumber = 1;
            const ushort pageSize = 2;
            var (termsPaginated, termDtoList) = GetStreetcodeObjects(pageSize);
            var request = GetRequest(UserRole.User, pageNumber, pageSize);

            MockHelpers.SetupMockStreetcodeRepositoryGetAllPaginated(_mockRepository, termsPaginated);
            MockHelpers.SetupMockMapper<IEnumerable<StreetcodeShortDto>, IEnumerable<StreetcodeContent>>(
                _mockMapper,
                termDtoList,
                termsPaginated.Entities);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.StreetcodesShort.Should().NotBeEmpty();
            result.Value.TotalAmount.Should().Be(pageSize);
        }

        [Fact]
        public async Task ShouldGetAllPaginatedFailingly_WhenPageNumberIsTooBig()
        {
            // Arrange
            const ushort pageNumber = 99;
            const ushort pageSize = 2;
            var (emptyTermsPaginated, emptyTermDtoList) = GetEmptyStreetcodeObjects();
            var request = GetRequest(UserRole.User, pageNumber, pageSize);
            var expectedErrorMessage = _mockNoSharedResourceLocalizer["NoStreetcodesExistNow"].Value;

            MockHelpers.SetupMockStreetcodeRepositoryGetAllPaginated(_mockRepository, emptyTermsPaginated);
            MockHelpers.SetupMockMapper(_mockMapper, emptyTermDtoList, emptyTermsPaginated.Entities);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Be(expectedErrorMessage);
            _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public async Task ShouldGetAllPaginatedFailingly_WhenPageNumberOrSizeIsZero(ushort pageNumber, ushort pageSize)
        {
            // Arrange
            var (emptyTermsPaginated, emptyTermDtoList) = GetEmptyStreetcodeObjects();
            var request = GetRequest(UserRole.User, pageNumber, pageSize);
            var expectedErrorMessage = _mockNoSharedResourceLocalizer["NoStreetcodesExistNow"].Value;

            MockHelpers.SetupMockStreetcodeRepositoryGetAllPaginated(_mockRepository, emptyTermsPaginated);
            MockHelpers.SetupMockMapper(_mockMapper, emptyTermDtoList, emptyTermsPaginated.Entities);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Be(expectedErrorMessage);
            _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
        }

        private static (PaginationResponse<StreetcodeContent>, List<StreetcodeShortDto>) GetStreetcodeObjects(int count)
        {
            var streetcodeContentsList = Enumerable
                .Range(0, count)
                .Select(i => new StreetcodeContent() { Id = i })
                .ToList();
            var streetcodeContentPaginated = PaginationResponse<StreetcodeContent>.Create(streetcodeContentsList.AsQueryable());

            var streetcodeShortDtoList = Enumerable
                .Range(0, count)
                .Select(i => new StreetcodeShortDto() { Id = streetcodeContentsList[i].Id })
                .ToList();

            return (streetcodeContentPaginated, streetcodeShortDtoList);
        }

        private static (PaginationResponse<StreetcodeContent>, List<StreetcodeShortDto>) GetEmptyStreetcodeObjects()
        {
            return (
                PaginationResponse<StreetcodeContent>.Create(new List<StreetcodeContent>().AsQueryable()),
                new List<StreetcodeShortDto>());
        }

        private static GetAllStreetcodesShortQuery GetRequest(UserRole userRole = UserRole.User, ushort? page = null, ushort? pageSize = null)
        {
            return new GetAllStreetcodesShortQuery(userRole, page, pageSize);
        }
    }
}