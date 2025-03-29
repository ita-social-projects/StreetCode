using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using FluentAssertions;
using Moq;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Jobs.GetActiveJobs;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Jobs
{
    public class GetActiveJobsHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private readonly GetActiveJobsHandler _handler;

        public GetActiveJobsHandlerTests()
        {
            _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILoggerService>();

            _handler = new GetActiveJobsHandler(
                _repositoryWrapperMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task ShouldReturnMappedActiveJobsDtos_CorrectPage()
        {
            // Arrange
            var jobs = new List<Job> 
            { 
                new Job { Id = 1, Title = "Developer", Status = true },
                new Job { Id = 2, Title = "Designer", Status = true }
            };
            var jobDtos = new List<JobDto> 
            { 
                new JobDto { Id = 1, Title = "Developer" },
                new JobDto { Id = 2, Title = "Designer" }
            };

            SetupMockRepositoryGetAllAsync(jobs);
            SetupMockMapper(jobDtos);

            // Act
            var result = await _handler.Handle(new GetActiveJobsQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(jobDtos);
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoActiveJobsFound()
        {
            // Arrange
            SetupMockRepositoryGetAllAsync(new List<Job>());
            SetupMockMapper(new List<JobDto>());

            // Act
            var result = await _handler.Handle(new GetActiveJobsQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldReturnFailureResult_WhenExceptionOccurs()
        {
            // Arrange
            var expectedErrorMessage = "Test exception";
            _repositoryWrapperMock
                .Setup(x => x.JobRepository.GetAllAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Job, bool>>?>(), null))
                .ThrowsAsync(new System.Exception(expectedErrorMessage));

            // Act
            var result = await _handler.Handle(new GetActiveJobsQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();

            result.Errors.Should().ContainSingle(error => error.Message == expectedErrorMessage);

            _loggerMock.Verify(x => x.LogError(It.IsAny<GetActiveJobsQuery>(), expectedErrorMessage), Times.Once);
        }

        private void SetupMockRepositoryGetAllAsync(IEnumerable<Job> jobs)
        {
            _repositoryWrapperMock
                .Setup(x => x.JobRepository.GetAllAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Job, bool>>?>(), null))
                .ReturnsAsync(jobs);
        }

        private void SetupMockMapper(IEnumerable<JobDto> jobDtos)
        {
            _mapperMock.Setup(x => x.Map<IEnumerable<JobDto>>(It.IsAny<IEnumerable<Job>>()))
                .Returns(jobDtos);
        }
    }
}
