using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentResults;
using Moq;
using Streetcode.BLL.MediatR.Jobs.GetById;
using Streetcode.BLL.SharedResource;
using Microsoft.EntityFrameworkCore.Query;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Jobs.Update;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.JobsTests
{
    public class UpdateJobTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> mockLocalizerFailedToUpdate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> mockLocalizerConvertNull;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFindSharedResource;

        public UpdateJobTest()
        {
            this._mockRepository = new();
            this.mockMapper = new();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFindSharedResource = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            this.mockLocalizerFailedToUpdate = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
            this.mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Fact]
        public async Task UpdateJobHandler_ExistingJobIsNull_ReturnsErrorMessage()
        {
            // Arrange
            string jobTitle = "Tested Job";
            var jobUpdateDto = GetJobUpdateDto();
            jobUpdateDto.Title = jobTitle;
            jobUpdateDto.Id = 0;

            this.SetupMapper(null, jobUpdateDto);

            var handler = new UpdateJobHandler(
                this._mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerFailedToUpdate.Object);

            this._mockRepository
                .Setup(p => p.JobRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Job, bool>>>(),
                    It.IsAny<Func<IQueryable<Job>, IIncludableQueryable<Job, object>>>()))
                .ReturnsAsync((Job?)null);

            this.mockLocalizerCannotFindSharedResource.Setup(x => x["CannotFindJobWithCorrespondingId", jobUpdateDto.Id])
                .Returns(new LocalizedString("CannotFindJobWithCorrespondingId", "Cannot find any job by the corresponding id: 0"));

            // Act
            var result = await handler.Handle(new UpdateJobCommand(jobUpdateDto), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Cannot find any job by the corresponding id: 0", result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldThrowException_SaveChangesAsyncIsNotSuccessful(int returnNumber)
        {
            // Arrange
            this.SetupCreateRepository(returnNumber);
            string jobTitle = "Tested Job";
            var jobUpdateDto = GetJobUpdateDto();
            jobUpdateDto.Title = jobTitle;
            jobUpdateDto.Id = 1;
            this.SetupMapper(new Job(), jobUpdateDto);

            var handler = new UpdateJobHandler(
                this._mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerFailedToUpdate.Object);

            this._mockRepository
                .Setup(p => p.JobRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Job, bool>>>(),
                    It.IsAny<Func<IQueryable<Job>, IIncludableQueryable<Job, object>>>()))
                .ReturnsAsync(new Job());

            var expectedError = "Failed to update job";
            this.mockLocalizerFailedToUpdate.Setup(x => x["FailedToUpdateJob"])
                .Returns(new LocalizedString("FailedToUpdateJob", expectedError));

            // Act
            var result = await handler.Handle(new UpdateJobCommand(jobUpdateDto), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldThrowException_TryMapNullRequest(int returnNumber)
        {
            // Arrange
            this.SetupCreateRepository(returnNumber);
            string jobTitle = "Tested Job";
            var jobUpdateDto = GetJobUpdateDto();
            jobUpdateDto.Title = jobTitle;
            jobUpdateDto.Id = 1;
            this.SetupMapper(null, jobUpdateDto);

            var handler = new UpdateJobHandler(
                this._mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerFailedToUpdate.Object);

            this._mockRepository
                .Setup(p => p.JobRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Job, bool>>>(),
                    It.IsAny<Func<IQueryable<Job>, IIncludableQueryable<Job, object>>>()))
                .ReturnsAsync(new Job());

            var expectedError = "Cannot convert null to job";
            this.mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToJob"])
                .Returns(new LocalizedString("CannotConvertNullToJob", expectedError));

            // Act
            var result = await handler.Handle(new UpdateJobCommand(jobUpdateDto), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }


        private static Job GetJob(int id = 0, string? title = null)
        {
            return new Job()
            {
                Id = id,
                Title = title ?? "Default Title",
            };
        }

        private static JobUpdateDto GetJobUpdateDto()
        {
            return new JobUpdateDto();
        }

        private void SetupCreateRepository(int returnNumber)
        {
            this._mockRepository.Setup(x => x.JobRepository.Create(It.IsAny<Job>()));
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupMapper(Job? testJob, JobUpdateDto testJobUpdateDto)
        {
            this.mockMapper.Setup(x => x.Map<Job>(It.IsAny<JobUpdateDto>()))
                .Returns(testJob);
            this.mockMapper.Setup(x => x.Map<JobUpdateDto>(It.IsAny<Job>()))
                .Returns(testJobUpdateDto);
        }
    }

}
