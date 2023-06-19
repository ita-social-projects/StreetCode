using AutoMapper;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NuGet.Frameworks;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Newss.GetAll;
using Streetcode.BLL.MediatR.Newss.SortedByDateTime;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Newss
{
    public class SortedByDateTimeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IBlobService> _blob;

        public SortedByDateTimeHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _blob = new Mock<IBlobService>();
        }

        [Theory]
        [InlineData("base64-encoded-audio")]
        public async Task Handle_ReturnsSuccess(string expectedBase64)
        {
            // arrange
            var testNews = new List<News>
            {
                new News { Id = 1, CreationDate = new DateTime(2023, 6, 1, 10, 25, 0) },
                new News { Id = 2, CreationDate = new DateTime(2022, 5, 1, 12, 56, 12) },
                new News { Id = 3, CreationDate = new DateTime(2024, 7, 1, 13, 17, 0) },
                new News { Id = 4, CreationDate = new DateTime(2023, 1, 1, 22, 2, 2) },
                new News { Id = 5, CreationDate = new DateTime(2023, 6, 1, 10, 24, 0) }
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
            var handler = new SortedByDateTimeHandler(_repository.Object, _mapper.Object, _blob.Object);

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
            var handler = new SortedByDateTimeHandler(_repository.Object, _mapper.Object, _blob.Object);

            // act
            var result = await handler.Handle(new SortedByDateTimeQuery(), CancellationToken.None);

            // assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        private void RepositorySetup(List<News> news)
        {
            _repository.Setup(repo => repo.NewsRepository
                .GetAllAsync(It.IsAny<Expression<Func<News, bool>>>(), It.IsAny<Func<IQueryable<News>, IIncludableQueryable<News, object>>>()))
                .ReturnsAsync(news);
        }
        private void MapperSetup(List<News> news)
        {
            _mapper.Setup(x => x.Map<IEnumerable<NewsDTO>>(It.IsAny<IEnumerable<News>>()))
                .Returns((IEnumerable<News> source) => source.Select(n => new NewsDTO { Id = n.Id, CreationDate = n.CreationDate }));
        }
        private void BlobSetup(string? expectedBase64)
        {
            _blob.Setup(blob => blob.FindFileInStorageAsBase64(It.IsAny<string>()))
                .Returns(expectedBase64);
        }
    }
}
