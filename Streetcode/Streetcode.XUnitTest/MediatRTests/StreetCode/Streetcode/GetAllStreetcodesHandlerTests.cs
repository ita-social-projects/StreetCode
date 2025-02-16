using System.Reflection;
using AutoMapper;
using FluentResults;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using Model = Streetcode.DAL.Entities.Streetcode.StreetcodeContent;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class GetAllStreetcodesHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
    private readonly Mock<IStreetcodeRepository> _streetcodeRepositoryMock;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<ILoggerService> _logger;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly MockFailedToValidateLocalizer _mockFailedToValidateLocalizer;

    public GetAllStreetcodesHandlerTests()
    {
        _streetcodeRepositoryMock = new Mock<IStreetcodeRepository>();
        _logger = new Mock<ILoggerService>();
        _mockCannotFindLocalizer = new MockCannotFindLocalizer();
        _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        _mapper = new Mock<IMapper>();
        _mockFailedToValidateLocalizer = new MockFailedToValidateLocalizer();
    }

    [Fact]
    public async Task Handle_ReturnsSuccess()
    {
        // Arrange
        var mockStreetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Title = "Title 1" },
            new () { Id = 2, Title = "Title 2" },
        };

        var request = new GetAllStreetcodesRequestDTO();
        var query = new GetAllStreetcodesQuery(request);
        var handler = SetupMockObjectsAndGetHandler(mockStreetcodes);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.NotEmpty(result.Value.Streetcodes);
            Assert.IsAssignableFrom<GetAllStreetcodesResponseDTO>(result.Value);
            Assert.Equal(2, result.Value.TotalAmount);
        });
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_And_CorrectPaginationParams()
    {
        // Arrange
        var mockStreetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Title = "Title 1" },
            new () { Id = 2, Title = "Title 2" },
        };

        var request = new GetAllStreetcodesRequestDTO { Page = 1, Amount = 1 };
        var query = new GetAllStreetcodesQuery(request);

        var handler = SetupMockObjectsAndGetHandler(mockStreetcodes);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.NotEmpty(result.Value.Streetcodes);
            Assert.IsAssignableFrom<GetAllStreetcodesResponseDTO>(result.Value);
            Assert.Equal(mockStreetcodes.Count, result.Value.TotalAmount);
            Assert.InRange(result.Value.Streetcodes.Count(), 0, request.Amount.Value);
            Assert.Single(result.Value.Streetcodes);
        });
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenNoStreetcodesExist()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO { Page = 1, Amount = 5 };
        var query = new GetAllStreetcodesQuery(request);
        var handler = SetupMockObjectsAndGetHandler();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Streetcodes);
            Assert.IsAssignableFrom<GetAllStreetcodesResponseDTO>(result.Value);
            Assert.Equal(0, result.Value.TotalAmount);
        });
    }

    [Fact]
    public async Task Handle_AppliesPagination_Correctly()
    {
        // Arrange
        var mockStreetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Title = "Title 1" },
            new () { Id = 2, Title = "Title 2" },
            new () { Id = 3, Title = "Title 3" },
            new () { Id = 4, Title = "Title 4" },
        };

        var request = new GetAllStreetcodesRequestDTO { Page = 2, Amount = 2 };
        var query = new GetAllStreetcodesQuery(request);
        var handler = SetupMockObjectsAndGetHandler(mockStreetcodes);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.NotEmpty(result.Value.Streetcodes);
            Assert.Equal(2, result.Value.Streetcodes.Count());
            Assert.Equal(mockStreetcodes[2].Id, result.Value.Streetcodes.First().Id);
            Assert.Equal(mockStreetcodes[3].Id, result.Value.Streetcodes.Last().Id);
        });
    }

    [Fact]
    public async Task Handle_ReturnSuccess_And_StreetCodesWithMatchTitle()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO
        {
            Title = "Some Title",
        };

        var query = new GetAllStreetcodesQuery(request);

        var mockStreetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Title = "Some Title" },
            new () { Id = 2, Title = "Some Title" },
            new () { Id = 3, Title = "Another Title" },
        };

        var handler = SetupMockObjectsAndGetHandler(mockStreetcodes);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<GetAllStreetcodesResponseDTO>(result.Value);
            Assert.NotNull(result.Value);
            Assert.NotNull(result.Value.Streetcodes);
            Assert.Equal(2, result.Value.Streetcodes.Count());
            Assert.Equal(2, result.Value.TotalAmount);
        });
    }

    [Fact]
    public async Task Handle_ReturnEmptyList_WhenTitleDoesNotExist()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO
        {
            Title = "Some Title",
        };

        var query = new GetAllStreetcodesQuery(request);

        var mockStreetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Title = "Another Title" },
        };

        var handler = SetupMockObjectsAndGetHandler(mockStreetcodes);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<GetAllStreetcodesResponseDTO>(result.Value);
            Assert.NotNull(result.Value);
            Assert.NotNull(result.Value.Streetcodes);
            Assert.Equal(0, result.Value.TotalAmount);
        });
    }

    [Fact]
    public async Task Handle_ReturnSortedStreetcodes()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO
        {
            Sort = "-Title",
        };

        var query = new GetAllStreetcodesQuery(request);

        var mockStreetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Title = "Streetcode 1" },
            new () { Id = 2, Title = "Streetcode 2" },
            new () { Id = 3, Title = "Streetcode 3" },
        };

        var handler = SetupMockObjectsAndGetHandler(mockStreetcodes);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<GetAllStreetcodesResponseDTO>(result.Value);
            Assert.NotNull(result.Value);
            Assert.NotNull(result.Value.Streetcodes);
            Assert.Equal(3, result.Value.TotalAmount);

            var resultList = result.Value.Streetcodes.ToList();
            Assert.Equal("Streetcode 3", resultList[0].Title);
            Assert.Equal("Streetcode 2", resultList[1].Title);
            Assert.Equal("Streetcode 1", resultList[2].Title);
        });
    }

    [Fact]
    public async Task Handle_ReturnsError_WhenIncorrectSortColum()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO
        {
            Sort = "-IncorrectSortColumn",
        };

        var query = new GetAllStreetcodesQuery(request);

        var mockStreetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Title = "Streetcode 1" },
            new () { Id = 2, Title = "Streetcode 2" },
        };

        var handler = SetupMockObjectsAndGetHandler(mockStreetcodes);
        var expectedErrorMessage = _mockCannotFindLocalizer["CannotFindAnyPropertyWithThisName"];

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains(expectedErrorMessage, result.Errors.Single().Message);
        });
    }

    [Fact]
    public void FindSortedStreetcodes_ReturnsSortedStreetcodes()
     {
         // Arrange
         var request = new GetAllStreetcodesRequestDTO
         {
             Sort = "-Title",
         };

         var handler = SetupMockObjectsAndGetHandler();

         var streetcodes = new List<StreetcodeContent>
         {
             new () { Id = 1, Title = "Streetcode 1" },
             new () { Id = 2, Title = "Streetcode 2" },
             new () { Id = 3, Title = "Streetcode 3" },
         }.AsQueryable();

         // Assert
         var methodInfo = typeof(GetAllStreetcodesHandler).GetMethod("FindSortedStreetcodes", BindingFlags.NonPublic | BindingFlags.Static);
         var parameters = new object[] { streetcodes, request.Sort };
         methodInfo?.Invoke(handler, parameters);
         var sortedStreetcodes = (IQueryable<StreetcodeContent>)parameters[0];

         // Act
         var resultList = sortedStreetcodes.ToList();
         Assert.Multiple(() =>
         {
             Assert.Equal("Streetcode 3", resultList[0].Title);
             Assert.Equal("Streetcode 2", resultList[1].Title);
             Assert.Equal("Streetcode 1", resultList[2].Title);
         });
     }

    [Fact]
    public void FindSortedStreetcodes_ReturnsError_WhenIncorrectSortColum()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO
        {
            Sort = "-IncorrectSortColumn",
        };

        var handler = SetupMockObjectsAndGetHandler();

        var streetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Title = "Streetcode 1" },
            new () { Id = 2, Title = "Streetcode 2" },
        }.AsQueryable();
        const string expectedErrorMessage = "CannotFindAnyPropertyWithThisName";

        // Act
        MethodInfo? methodInfo = typeof(GetAllStreetcodesHandler).GetMethod("FindSortedStreetcodes", BindingFlags.NonPublic | BindingFlags.Static);
        var parameters = new object[] { streetcodes, request.Sort };
        var result = methodInfo?.Invoke(handler, parameters);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            var typedResult = (Result)result;
            Assert.True(typedResult.IsFailed);
            Assert.Contains(expectedErrorMessage, typedResult.Errors.Single().Message);
        });
    }

    [Fact]
    public void FindFilteredStreetcodes_ReturnsFilteredStreetcodes()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO
        {
            Filter = "Teaser:Streetcode",
        };

        var handler = SetupMockObjectsAndGetHandler();

        var streetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Teaser = "Streetcode 1" },
            new () { Id = 2, Teaser = "Streetcode 2" },
        }.AsQueryable();

        // Act
        var methodInfo = typeof(GetAllStreetcodesHandler).GetMethod("FindFilteredStreetcodes", BindingFlags.NonPublic | BindingFlags.Instance);
        var parameters = new object[] { streetcodes, request.Filter };
        methodInfo?.Invoke(handler, parameters);
        var filteredStreetcodes = (IQueryable<StreetcodeContent>)parameters[0];

        // Assert
        var resultList = filteredStreetcodes.ToList();
        Assert.Multiple(() =>
        {
            Assert.Equal("Streetcode 1", resultList[0].Teaser);
            Assert.Equal("Streetcode 2", resultList[1].Teaser);
        });
    }

    [Fact]
    public void FindFilteredStreetcodes_ReturnsError_WhenIncorrectFilterColum()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO
        {
            Filter = "IncorrectTeaser:Streetcode",
        };

        var handler = SetupMockObjectsAndGetHandler();

        var streetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Teaser = "Streetcode 1" },
        }.AsQueryable();

        // Act
        MethodInfo? methodInfo = typeof(GetAllStreetcodesHandler).GetMethod("FindFilteredStreetcodes", BindingFlags.NonPublic | BindingFlags.Static);
        var parameters = new object[] { streetcodes, request.Filter };
        var result = methodInfo?.Invoke(handler, parameters);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            var typedResult = (Result)result;
            Assert.True(typedResult.IsFailed);
        });
    }

    [Fact]
    public async Task Handle_ReturnFilteredStreetcodes()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO
        {
            Filter = "Teaser:Teaser",
        };

        var query = new GetAllStreetcodesQuery(request);

        var mockStreetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Teaser = "Streetcode 1" },
            new () { Id = 2, Teaser = "Teaser" },
        };

        var handler = SetupMockObjectsAndGetHandler(mockStreetcodes);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<GetAllStreetcodesResponseDTO>(result.Value);
            Assert.NotNull(result.Value);
            Assert.NotNull(result.Value.Streetcodes);
            Assert.Equal(1, result.Value.TotalAmount);
            var resultList = result.Value.Streetcodes.ToList();
            Assert.Equal("Teaser", resultList[0].Teaser);
        });
    }

    [Fact]
    public async Task Handle_ReturnsError_WhenIncorrectFilterColumn()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO
        {
            Filter = "IncorrectTeaser:Teaser",
        };

        var query = new GetAllStreetcodesQuery(request);

        var mockStreetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Teaser = "Streetcode 1" },
        };

        var handler = SetupMockObjectsAndGetHandler(mockStreetcodes);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
        });
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_And_EmptyList_WhenFilterValueDoesNotExist()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO
        {
            Filter = "Teaser:NonExistingFilterValue",
        };

        var query = new GetAllStreetcodesQuery(request);

        var mockStreetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Teaser = "Streetcode 1" },
        };

        var handler = SetupMockObjectsAndGetHandler(mockStreetcodes);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<GetAllStreetcodesResponseDTO>(result.Value);
            Assert.NotNull(result.Value);
            Assert.NotNull(result.Value.Streetcodes);
            Assert.Equal(0, result.Value.TotalAmount);
        });
    }

    [Theory]
    [InlineData(-10, 9)]
    [InlineData(1, -5)]
    [InlineData(0, 0)]
    public async Task Handle_ReturnsError_WhenInvalidPaginationParams(int page, int amount)
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO
        {
            Page = page,
            Amount = amount,
        };
        var expectedErrorMessage = _mockFailedToValidateLocalizer["InvalidPaginationParameters"];
        var query = new GetAllStreetcodesQuery(request);
        var handler = SetupMockObjectsAndGetHandler();

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains(expectedErrorMessage, result.Errors.Single().Message);
        });
    }

    private GetAllStreetcodesHandler SetupMockObjectsAndGetHandler(IEnumerable<StreetcodeContent>? mockStreetcodes = null)
    {
        mockStreetcodes ??= new List<StreetcodeContent>();

        _streetcodeRepositoryMock
            .Setup(repo => repo.FindAll(null, null))
            .Returns(mockStreetcodes.AsQueryable());

        _repositoryWrapperMock
            .Setup(x => x.StreetcodeRepository)
            .Returns(_streetcodeRepositoryMock.Object);

        _mapper
            .Setup(x => x.Map<IEnumerable<StreetcodeDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
            .Returns((IEnumerable<StreetcodeContent> src) =>
                src.Select(s => new StreetcodeDTO
                {
                    Id = s.Id,
                    Title = s.Title!,
                    Teaser = s.Teaser!,
                }).ToList());

        var handler = new GetAllStreetcodesHandler(_repositoryWrapperMock.Object, _mapper.Object, _logger.Object, _mockCannotFindLocalizer, _mockFailedToValidateLocalizer);

        return handler;
    }
}