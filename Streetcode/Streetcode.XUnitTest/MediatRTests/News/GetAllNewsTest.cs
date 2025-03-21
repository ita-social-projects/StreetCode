using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Newss.GetAll;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.News
{
    public class GetAllNewsTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly GetAllNewsHandler _handler;

        public GetAllNewsTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _handler = GetHandler();
        }

        [Fact]
        public async Task ShouldReturnPaginatedNews_CorrectPage()
        {
            // Arrange.
            ushort pageSize = 2;
            ushort pageNumber = 1;

            SetupMockObjects(pageNumber, pageSize, GetNewsDtOs(pageSize), GetEmptyHttpHeaders());
            var handler = GetHandler();

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

            SetupMockObjects(pageNumber, pageSize, GetNewsDtOs(0), GetEmptyHttpHeaders());

            // Act.
            var result = await _handler.Handle(new GetAllNewsQuery(pageSize, pageNumber), CancellationToken.None);

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

            SetupMockObjects(pageNumber, pageSize, GetNewsDtOs(0), GetEmptyHttpHeaders());

            // Act.
            var result = await _handler.Handle(new GetAllNewsQuery(pageSize, pageNumber), CancellationToken.None);

            // Assert.
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Empty(result.Value.News));
        }

        private void SetupMockRepositoryGetAllPaginatedAsync(ushort pageNumber, ushort pageSize)
        {
            _mockRepository
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

        private static IEnumerable<NewsDTO> GetNewsDtOs(ushort count)
        {
            var newsDto = Enumerable
                .Range(0, count)
                .Select((news, index) => new NewsDTO() { Id = index });

            return newsDto;
        }

        private static IHeaderDictionary GetEmptyHttpHeaders()
        {
            return new HeaderDictionary();
        }

        private void SetupMockMapper(IEnumerable<NewsDTO> mapperReturnCollection)
        {
            _mockMapper
                .Setup(x => x.Map<IEnumerable<NewsDTO>>(It.IsAny<IEnumerable<DAL.Entities.News.News>>()))
                .Returns(mapperReturnCollection);
        }

        private void SetupMockHttpAccessorToReturnHeadersCollection(IHeaderDictionary headersCollection)
        {
            _mockHttpContextAccessor
                .Setup(accessor => accessor.HttpContext!.Response.Headers)
                .Returns(headersCollection);
        }

        private void SetupMockObjects(
            ushort pageNumber,
            ushort pageSize,
            IEnumerable<NewsDTO> mapperReturnCollection,
            IHeaderDictionary headersCollection)
        {
            SetupMockMapper(mapperReturnCollection);
            SetupMockHttpAccessorToReturnHeadersCollection(headersCollection);
            SetupMockRepositoryGetAllPaginatedAsync(pageNumber, pageSize);
        }

        private GetAllNewsHandler GetHandler() =>
            new GetAllNewsHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _blobService.Object,
                _mockHttpContextAccessor.Object);
    }
}
