using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Newtonsoft.Json;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Newss.GetAll;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.News
{
    public class GetAllNewsTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IBlobService> blobService;
        private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private readonly GetAllNewsHandler handler;

        public GetAllNewsTest()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.blobService = new Mock<IBlobService>();
            this.mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            this.handler = this.GetHandler();
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
                () => Assert.Equal(pageSize, result.Value.News.Count()));
        }

        [Fact]
        public async Task ShouldReturnEmptyCollection_PageNumberTooBig()
        {
            // Arrange.
            ushort pageSize = 2;
            ushort pageNumber = 99;

            this.SetupMockObjects(pageNumber, pageSize, GetNewsDTOs(0), GetEmptyHTTPHeaders());

            // Act.
            var result = await this.handler.Handle(new GetAllNewsQuery(pageSize, pageNumber), CancellationToken.None);

            // Assert.
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Empty(result.Value.News));
        }

        [Fact]
        public async Task ShouldReturnEmptyCollection_PageSizeIsZero()
        {
            // Arrange.
            ushort pageSize = 0;
            ushort pageNumber = 1;

            this.SetupMockObjects(pageNumber, pageSize, GetNewsDTOs(0), GetEmptyHTTPHeaders());

            // Act.
            var result = await this.handler.Handle(new GetAllNewsQuery(pageSize, pageNumber), CancellationToken.None);

            // Assert.
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Empty(result.Value.News));
        }

        private void SetupMockRepositoryGetAllPaginatedAsync(ushort pageNumber, ushort pageSize)
        {
            this.mockRepository
                .Setup(x => x.NewsRepository.GetAllPaginated(
                    It.IsAny<ushort>(),
                    It.IsAny<ushort>(),
                    null,
                    null,
                    It.IsAny<Func<IQueryable<DAL.Entities.News.News>, IIncludableQueryable<DAL.Entities.News.News, object>>?>(),
                    null,
                    It.IsAny<Expression<Func<DAL.Entities.News.News, object>>?>()))
                .Returns(GetPaginationResponse(pageNumber, pageSize));
        }

        private static PaginationResponse<DAL.Entities.News.News> GetPaginationResponse(ushort pageNumber, ushort pageSize)
        {
            var news = new List<DAL.Entities.News.News>
            {
                new DAL.Entities.News.News
                {
                    Id = 1,
                },
                new DAL.Entities.News.News
                {
                    Id = 2,
                },
                new DAL.Entities.News.News
                {
                    Id = 3,
                },
                new DAL.Entities.News.News
                {
                    Id = 4,
                },
            };

            return PaginationResponse<DAL.Entities.News.News>.Create(news.AsQueryable(), pageNumber, pageSize);
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

        private void SetupMockMapper(IEnumerable<NewsDTO> mapperReturnCollection)
        {
            this.mockMapper
                .Setup(x => x.Map<IEnumerable<NewsDTO>>(It.IsAny<IEnumerable<DAL.Entities.News.News>>()))
                .Returns(mapperReturnCollection);
        }

        private void SetupMockHttpAccessorToReturnHeadersCollection(IHeaderDictionary headersCollection)
        {
            this.mockHttpContextAccessor
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
                this.mockRepository.Object,
                this.mockMapper.Object,
                this.blobService.Object,
                this.mockHttpContextAccessor.Object);
    }
}
