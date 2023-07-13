using AutoMapper;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using NuGet.Frameworks;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Newss.GetAll;
using Streetcode.BLL.MediatR.Newss.SortedByDateTime;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using NewsModel = Streetcode.DAL.Entities.News.News;

namespace Streetcode.XUnitTest.MediatRTests.Newss
{
    public class SortedByDateTimeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IBlobService> _blob;
        private readonly Mock<IStringLocalizer<NoSharedResource>> _mockLocalizer;

        public SortedByDateTimeHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _blob = new Mock<IBlobService>();
            _mockLocalizer = new Mock<IStringLocalizer<NoSharedResource>>();
        }

        [Theory]
        [InlineData("base64-encoded-audio")]
        public async Task Handle_ReturnsSuccess(string expectedBase64)
        {
            // arrange
            var testNews = new List<NewsModel>
            {
                new NewsModel { Id = 1, CreationDate = new DateTime(2023, 6, 1, 10, 25, 0) },
                new NewsModel { Id = 2, CreationDate = new DateTime(2022, 5, 1, 12, 56, 12) },
                new NewsModel { Id = 3, CreationDate = new DateTime(2024, 7, 1, 13, 17, 0) },
                new NewsModel { Id = 4, CreationDate = new DateTime(2023, 1, 1, 22, 2, 2) },
                new NewsModel { Id = 5, CreationDate = new DateTime(2023, 6, 1, 10, 24, 0) }
            };

            var expectedNews = new List<NewsDTO>
            {
                new NewsDTO { Id = 3, CreationDate = new DateTime(2024, 7, 1, 13, 17, 0) },
                new NewsDTO { Id = 1, CreationDate = new DateTime(2023, 6, 1, 10, 25, 0) },
                new NewsDTO { Id = 5, CreationDate = new DateTime(2023, 6, 1, 10, 24, 0) },
                new NewsDTO { Id = 4, CreationDate = new DateTime(2023, 1, 1, 22, 2, 2) },
                new NewsDTO { Id = 2, CreationDate = new DateTime(2022, 5, 1, 12, 56, 12) }
            };

            RepositorySetup(testNews);
            MapperSetup(testNews);
            BlobSetup(expectedBase64);
            var handler = new SortedByDateTimeHandler(_repository.Object, _mapper.Object, _blob.Object, _mockLocalizer.Object);

            // act
            var result = await handler.Handle(new SortedByDateTimeQuery(), CancellationToken.None);

            //assert
            result.Value.Should().BeEquivalentTo(expectedNews);
        }

        [Fact]
        public async Task Handle_ReturnsError()
        {
            // arrange
            string expectedErrorMessage = "There are no news in the database";
            RepositorySetup(null);
            MapperSetup(null);
            BlobSetup(null);
            var handler = new SortedByDateTimeHandler(_repository.Object, _mapper.Object, _blob.Object, _mockLocalizer.Object);

            // act
            var result = await handler.Handle(new SortedByDateTimeQuery(), CancellationToken.None);

            // assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        private void RepositorySetup(List<NewsModel> news)
        {
            _repository.Setup(repo => repo.NewsRepository
                .GetAllAsync(It.IsAny<Expression<Func<NewsModel, bool>>>(), It.IsAny<Func<IQueryable<NewsModel>, IIncludableQueryable<NewsModel, object>>>()))
                .ReturnsAsync(news);
        }
        private void MapperSetup(List<NewsModel> news)
        {
            _mapper.Setup(x => x.Map<IEnumerable<NewsDTO>>(It.IsAny<IEnumerable<NewsModel>>()))
                .Returns((IEnumerable<NewsModel> source) => source.Select(n => new NewsDTO { Id = n.Id, CreationDate = n.CreationDate }));
        }
        private void BlobSetup(string? expectedBase64)
        {
            _blob.Setup(blob => blob.FindFileInStorageAsBase64(It.IsAny<string>()))
                .Returns(expectedBase64);
        }
    }
}
