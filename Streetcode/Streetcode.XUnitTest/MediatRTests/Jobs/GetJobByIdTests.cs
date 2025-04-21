using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Jobs.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Jobs
{
    public class GetJobByIdTests
    {
        private const int id = 1;
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;

        private readonly Job _job = new Job
        {
            Id = id,
            Title = "Developer",
            Status = true,
            Description = "Develop software",
            Salary = "5000",
        };

        private readonly JobDto _jobDto = new JobDto
        {
            Id = id,
            Title = "Developer",
        };

        public GetJobByIdTests()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_ExistingId()
        {
            // Arrange
            this.SetupRepository(_job);
            this.SetupMapper(_jobDto);

            var handler = new GetJobByIdHandler(mockMapper.Object, mockRepository.Object, mockLogger.Object,
                mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetJobByIdQuery(id, UserRole.Admin), CancellationToken.None);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<JobDto>();
            result.Value.Id.Should().Be(id);
        }

        [Fact]
        public async Task Handler_Returns_NoMatching_Element()
        {
            // Arrange
            this.SetupRepository(new Job());
            this.SetupMapper(new JobDto());

            var handler = new GetJobByIdHandler(mockMapper.Object, mockRepository.Object, mockLogger.Object,
                mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetJobByIdQuery(id, UserRole.Admin), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<JobDto>(result.Value),
                () => Assert.False(result.Value.Id.Equals(id)));
        }

        private void SetupRepository(Job job)
        {
            mockRepository
                .Setup(repo => repo.JobRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Job, bool>>>(),
                        It.IsAny<Func<IQueryable<Job>, IIncludableQueryable<Job, object>>>()))
                .ReturnsAsync(job);
        }

        private void SetupMapper(JobDto jobDto)
        {
            mockMapper
                .Setup(x => x.Map<JobDto>(It.IsAny<Job>()))
                .Returns(jobDto);
        }
    }
}