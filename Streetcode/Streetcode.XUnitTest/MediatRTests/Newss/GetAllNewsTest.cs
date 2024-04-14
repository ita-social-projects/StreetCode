using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Newtonsoft.Json;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Newss.GetAll;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Newss
{
    public class GetAllNewsTest
    {
        private Mock<IRepositoryWrapper> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

        public GetAllNewsTest()
        {
            this._mockRepository = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._blobService = new Mock<IBlobService>();
            this._mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        }

        [Fact]
        public async Task ShouldReturnPaginatedNews_CorrectPage()
        {
            // Arrange.
            ushort pageSize = 2;
            ushort pageNumber = 1;

            this.SetupMockObjects(pageNumber, pageSize, GetNewsDTOs(pageSize), GetEmptyHTTPHeaders());
            var handler = this.GetHandler();

            // Act.
            var result = await handler.Handle(new GetAllNewsQuery(pageSize, pageNumber), CancellationToken.None);

            // Assert.
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Equal(pageSize, result.Value.Count()));
        }

        [Fact]
        public async Task ShouldApplyPaginationHeaderWithCorrectData_CorrectPage()
        {
            // Arrange.
            ushort pageSize = 2;
            ushort pageNumber = 1;
            ushort totalItems = GetPaginationResponse(pageNumber, pageSize).TotalItems;
            string expectedPaginationHeader = this.GetPaginationHeaderInJSON(pageSize, pageNumber, totalItems);

            var httpHeaders = GetEmptyHTTPHeaders();
            this.SetupMockObjects(pageNumber, pageSize, GetNewsDTOs(pageSize), httpHeaders);
            var handler = this.GetHandler();

            // Act.
            var result = await handler.Handle(new GetAllNewsQuery(pageSize, pageNumber), CancellationToken.None);

            // Assert.
            Assert.Multiple(
                () => Assert.True(httpHeaders.Keys.Contains("x-pagination")),
                () => Assert.Equal(expectedPaginationHeader, httpHeaders["x-pagination"]));
        }

        [Fact]
        public async Task ShouldReturnEmptyCollection_PageNumberTooBig()
        {
            // Arrange.
            ushort pageSize = 2;
            ushort pageNumber = 99;

            this.SetupMockObjects(pageNumber, pageSize, GetNewsDTOs(0), GetEmptyHTTPHeaders());
            var handler = this.GetHandler();

            // Act.
            var result = await handler.Handle(new GetAllNewsQuery(pageSize, pageNumber), CancellationToken.None);

            // Assert.
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Empty(result.Value));
        }

        [Fact]
        public async Task ShouldReturnEmptyCollection_PageSizeIsZero()
        {
            // Arrange.
            ushort pageSize = 0;
            ushort pageNumber = 1;

            this.SetupMockObjects(pageNumber, pageSize, GetNewsDTOs(0), GetEmptyHTTPHeaders());
            var handler = this.GetHandler();

            // Act.
            var result = await handler.Handle(new GetAllNewsQuery(pageSize, pageNumber), CancellationToken.None);

            // Assert.
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Empty(result.Value));
        }

        private void SetupMockRepositoryGetAllPaginatedAsync(ushort pageNumber, ushort pageSize)
        {
            this._mockRepository
                .Setup(x => x.NewsRepository.GetAllPaginated(
                    It.IsAny<ushort>(),
                    It.IsAny<ushort>(),
                    null,
                    null,
                    It.IsAny<Func<IQueryable<News>, IIncludableQueryable<News, object>>?>(),
                    null,
                    It.IsAny<Expression<Func<News, object>>?>()))
                .Returns(GetPaginationResponse(pageNumber, pageSize));
        }

        private void SetupMockMapper(IEnumerable<NewsDTO> mapperReturnCollection)
        {
            this._mockMapper
                .Setup(x => x.Map<IEnumerable<NewsDTO>>(It.IsAny<IEnumerable<News>>()))
                .Returns(mapperReturnCollection);
        }

        private void SetupMockHttpAccessorToReturnHeadersCollection(IHeaderDictionary headersCollection)
        {
            this._mockHttpContextAccessor
                .Setup(accessor => accessor.HttpContext!.Response.Headers)
                .Returns(headersCollection);
        }

        private void SetupMockObjects(
            ushort pageNumber,
            ushort pageSize,
            IEnumerable<NewsDTO> mapperReturnCollection,
            IHeaderDictionary headersCollection)
        {
            this.SetupMockMapper(mapperReturnCollection);
            this.SetupMockHttpAccessorToReturnHeadersCollection(headersCollection);
            this.SetupMockRepositoryGetAllPaginatedAsync(pageNumber, pageSize);
        }

        private static PaginationResponse<News> GetPaginationResponse(ushort pageNumber, ushort pageSize)
        {
            var news = new List<News>
            {
                new News
                {
                    Id = 1,
                },
                new News
                {
                    Id = 2,
                },
                new News
                {
                    Id = 3,
                },
                new News
                {
                    Id = 4,
                },
            };

            return PaginationResponse<News>.Create(news.AsQueryable(), pageNumber, pageSize);
        }

        private static IEnumerable<NewsDTO> GetNewsDTOs(ushort count)
        {
            var newsDTO = Enumerable
                .Range(0, count)
                .Select((news, index) => new NewsDTO() { Id = index });

            return newsDTO;
        }

        private static IHeaderDictionary GetEmptyHTTPHeaders()
        {
            return new HeaderDictionary();
        }

        private string GetPaginationHeaderInJSON(ushort pageSize, ushort currentPage, ushort totalItems)
        {
            ushort totalPages = totalItems % pageSize == 0 ? (ushort)(totalItems / pageSize) : (ushort)((totalItems / pageSize) + 1);
            var metadata = new
            {
                CurrentPage = currentPage,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalItems = totalItems,
            };

            return JsonConvert.SerializeObject(metadata);
        }

        private GetAllNewsHandler GetHandler() =>
            new GetAllNewsHandler(
                this._mockRepository.Object,
                this._mockMapper.Object,
                this._blobService.Object,
                this._mockHttpContextAccessor.Object);
    }
}
