using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Jobs.GetAll;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Jobs
{
    public class GetAllJobsTests
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly GetAllJobsHandler handler;

        public GetAllJobsTests()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();

            this.handler = new GetAllJobsHandler(
                mockRepository.Object,
                mockMapper.Object,
                mockLogger.Object);
        }

        [Fact]
        public async Task ShouldReturnPaginatedJobs_CorrectPage()
        {
            // Arrange
            ushort pageSize = 2;
            ushort pageNumber = 1;

            this.SetupMockObjects(pageNumber, pageSize, GetJobsDTOs(pageSize), GetEmptyHTTPHeaders());

            // Act
            var result = await handler.Handle(new GetAllJobsQuery(UserRole.Admin, pageSize, pageNumber), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Equal(pageSize, result.Value.Jobs.Count()));
        }

        [Fact]
        public async Task ShouldReturnEmptyCollection_PageNumberTooBig()
        {
            // Arrange.
            ushort pageSize = 2;
            ushort pageNumber = 9999;

            this.SetupMockObjects(pageNumber, pageSize, GetJobsDTOs(0), GetEmptyHTTPHeaders());

            // Act.
            var result = await this.handler.Handle(new GetAllJobsQuery(UserRole.Admin, pageSize, pageNumber), CancellationToken.None);

            // Assert.
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Empty(result.Value.Jobs));
        }

        [Fact]
        public async Task ShouldReturnEmptyCollection_PageSizeIsZero()
        {
            // Arrange.
            ushort pageSize = 0;
            ushort pageNumber = 1;

            this.SetupMockObjects(pageNumber, pageSize, GetJobsDTOs(0), GetEmptyHTTPHeaders());

            // Act.
            var result = await this.handler.Handle(new GetAllJobsQuery(UserRole.Admin, pageSize, pageNumber), CancellationToken.None);

            // Assert.
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Empty(result.Value.Jobs));
        }

        private void SetupMockObjects(
            ushort pageNumber,
            ushort pageSize,
            IEnumerable<JobDto> mapperReturnCollection,
            IHeaderDictionary headersCollection)
        {
            this.SetupMockMapper(mapperReturnCollection);
            this.SetupMockRepositoryGetAllPaginatedAsync(pageNumber, pageSize);
        }

        private void SetupMockMapper(IEnumerable<JobDto> mapperReturnCollection)
        {
            this.mockMapper
                .Setup(x => x.Map<IEnumerable<JobDto>>(It.IsAny<IEnumerable<DAL.Entities.Jobs.Job>>()))
                .Returns(mapperReturnCollection);
        }

        private void SetupMockRepositoryGetAllPaginatedAsync(ushort pageNumber, ushort pageSize)
        {
            this.mockRepository
                .Setup(x => x.JobRepository.GetAllPaginated(
                    It.IsAny<ushort>(),
                    It.IsAny<ushort>(),
                    null,
                    null,
                    It.IsAny<Func<IQueryable<DAL.Entities.Jobs.Job>,
                        IIncludableQueryable<DAL.Entities.Jobs.Job, object>>?>(),
                    null,
                    It.IsAny<Expression<Func<DAL.Entities.Jobs.Job, object>>?>()))
                .Returns(GetPaginationResponse(pageNumber, pageSize));
        }

        private static PaginationResponse<DAL.Entities.Jobs.Job> GetPaginationResponse(ushort pageNumber,
            ushort pageSize)
        {
            var jobs = new List<DAL.Entities.Jobs.Job>
            {
                new DAL.Entities.Jobs.Job { Id = 1 },
                new DAL.Entities.Jobs.Job { Id = 2 },
                new DAL.Entities.Jobs.Job { Id = 3 },
                new DAL.Entities.Jobs.Job { Id = 4 },
            };

            return PaginationResponse<DAL.Entities.Jobs.Job>.Create(jobs.AsQueryable(), pageNumber, pageSize);
        }

        private static IEnumerable<JobDto> GetJobsDTOs(ushort count)
        {
            return Enumerable
                .Range(0, count)
                .Select((job, index) => new JobDto { Id = index });
        }

        private static IHeaderDictionary GetEmptyHTTPHeaders()
        {
            return new HeaderDictionary();
        }
    }
}