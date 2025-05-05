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
using Streetcode.BLL.MediatR.Jobs.Delete;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;


namespace Streetcode.XUnitTest.MediatRTests.Jobs
{
    public class DeleteJobTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public DeleteJobTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldDeleteSuccessfully_ExistingJob()
        {
            // Arrange
            var jobToDelete = DeleteJob(); 
            SetupMockRepositoryGetFirstOrDefault(jobToDelete); 
            SetupMockRepositorySaveChangesReturns(1); 

            var handler = new DeleteJobHandler(_mockRepository.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new DeleteJobCommand(jobToDelete.Id), CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue(); 
            _mockRepository.Verify(
                x => x.JobRepository.Delete(It.Is<Job>(x => x.Id == jobToDelete.Id)), 
                Times.Once); 
            _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once); 
        }

        [Fact]
        public async Task ShouldReturnFailure_WhenJobNotFound()
        {
            // Arrange
            SetupMockRepositoryGetFirstOrDefault(null); 

            var handler = new DeleteJobHandler(_mockRepository.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new DeleteJobCommand(999), CancellationToken.None); 

            // Assert
            result.IsFailed.Should().BeTrue(); 
            _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Never); 
        }

        private static Job DeleteJob()
        {
            return new Job
            {
                Id = 1,
                Title = "Developer",
                Status = true,
                Description = "Develop software",
                Salary = "5000"
            };
        }

        private void SetupMockRepositoryGetFirstOrDefault(Job? job)
        {
            _mockRepository
                .Setup(x => x.JobRepository
                    .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Job, bool>>>(), null))
                .ReturnsAsync(job); 
        }

        private void SetupMockRepositorySaveChangesReturns(int number)
        {
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(number); 
        }
    }
}
