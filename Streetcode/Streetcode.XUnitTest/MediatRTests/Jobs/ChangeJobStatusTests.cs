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
using Streetcode.BLL.MediatR.Jobs.ChangeStatus;
using Streetcode.BLL.SharedResource;
using Microsoft.EntityFrameworkCore.Query;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Jobs.Update;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;


public class ChangeJobStatusHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;
    
    public ChangeJobStatusHandlerTests()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task Handle_JobNotFound_ReturnsFailure()
    {
        // Arrange
        var request = new ChangeJobStatusCommand(new JobChangeStatusDto { Id = 1, Status = true });
        _mockRepository.Setup(r => r.JobRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Job, bool>>>(), null))
            .ReturnsAsync((Job?)null);
        _mockLocalizer.Setup(l => l["CannotFindJobWithCorrespondingId", request.jobChangeStatusDto.Id])
            .Returns(new LocalizedString("CannotFindJobWithCorrespondingId", "Cannot find job with ID 1"));
        
        var handler = new ChangeJobStatusHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizer.Object);
        
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be("Cannot find job with ID 1");
    }

    [Fact]
    public async Task Handle_JobAlreadyHasSameStatus_ReturnsSuccess()
    {
        // Arrange
        var job = new Job { Id = 1, Status = true };
        var request = new ChangeJobStatusCommand(new JobChangeStatusDto { Id = 1, Status = true });
        _mockRepository.Setup(r => r.JobRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Job, bool>>>(), null))
            .ReturnsAsync(job);
        
        var handler = new ChangeJobStatusHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizer.Object);
        
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(1);
    }

    [Fact]
    public async Task Handle_JobStatusUpdatedSuccessfully_ReturnsSuccess()
    {
        // Arrange
        var job = new Job { Id = 1, Status = false };
        var request = new ChangeJobStatusCommand(new JobChangeStatusDto { Id = 1, Status = true });
        _mockRepository.Setup(r => r.JobRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Job, bool>>>(), null))
            .ReturnsAsync(job);
        _mockRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
        
        var handler = new ChangeJobStatusHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizer.Object);
        
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(1);
        _mockRepository.Verify(r => r.JobRepository.Update(job), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_SaveChangesFails_ReturnsFailure()
    {
        // Arrange
        var job = new Job { Id = 1, Status = false };
        var request = new ChangeJobStatusCommand(new JobChangeStatusDto { Id = 1, Status = true });
        _mockRepository.Setup(r => r.JobRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Job, bool>>>(), null))
            .ReturnsAsync(job);
        _mockRepository.Setup(r => r.SaveChangesAsync()).ThrowsAsync(new Exception("Database error"));
        
        var handler = new ChangeJobStatusHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizer.Object);
        
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be("Database error");
        _mockLogger.Verify(l => l.LogError(request, "Database error"), Times.Once);
    }
}
