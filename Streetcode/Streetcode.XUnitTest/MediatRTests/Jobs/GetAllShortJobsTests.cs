using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentResults;
using Moq;
using Microsoft.EntityFrameworkCore.Query;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Jobs.GetAll;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Jobs
{
    public class GetAllShortJobsHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private readonly GetAllShortJobsHandler _handler;

        public GetAllShortJobsHandlerTests()
        {
            _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILoggerService>();

            _handler = new GetAllShortJobsHandler(
                _repositoryWrapperMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsMappedJobShortDtos()
        {
            // Arrange
            var jobs = new List<Job> { new Job { Id = 1, Title = "Developer" } };
            var jobShortDtos = new List<JobShortDto> { new JobShortDto { Id = 1, Title = "Developer" } };

            SetupMockRepositoryGetAllAsync(jobs);
            SetupMockMapper(jobShortDtos);

            // Act
            var result = await _handler.Handle(new GetAllShortJobsQuery(UserRole.Admin), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(jobShortDtos);
        }

        [Fact]
        public async Task Handle_WhenNoJobsFound_ReturnsEmptyList()
        {
            // Arrange
            SetupMockRepositoryGetAllAsync(new List<Job>());
            SetupMockMapper(new List<JobShortDto>());

            // Act
            var result = await _handler.Handle(new GetAllShortJobsQuery(UserRole.Admin), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEmpty();
        }

        private void SetupMockRepositoryGetAllAsync(IEnumerable<Job> jobs)
        {
            _repositoryWrapperMock
                .Setup(x => x.JobRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Job, bool>>?>() ?? null,
                    It.IsAny<Func<IQueryable<Job>, IIncludableQueryable<Job, object>>?>() ?? null))
                .ReturnsAsync(jobs);
        }


        private void SetupMockMapper(IEnumerable<JobShortDto> jobShortDtos)
        {
            _mapperMock.Setup(x => x.Map<IEnumerable<JobShortDto>>(It.IsAny<IEnumerable<Job>>()))
                .Returns(jobShortDtos);
        }
    }
}