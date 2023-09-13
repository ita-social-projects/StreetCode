using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Jobs.Delete
{
	public class DeleteJobHandler : IRequestHandler<DeleteJobCommand, Result<int>>
	{
		private readonly IRepositoryWrapper _repositoryWrapper;
		private readonly ILoggerService _logger;

		public DeleteJobHandler(IRepositoryWrapper repository, ILoggerService logger)
		{
			_repositoryWrapper = repository;
			_logger = logger;
		}

		public async Task<Result<int>> Handle(DeleteJobCommand request, CancellationToken cancellationToken)
		{
			var jobToDelete =
				await _repositoryWrapper.JobRepository.GetFirstOrDefaultAsync(x => x.Id == request.id);
			if (jobToDelete is null)
			{
				string exMessage = $"No job found by entered Id - {request.id}";
				_logger.LogError(request, exMessage);
				return Result.Fail(exMessage);
			}

			try
			{
				_repositoryWrapper.JobRepository.Delete(jobToDelete);
				await _repositoryWrapper.SaveChangesAsync();
				return Result.Ok(request.id);
			}
			catch(Exception ex)
			{
				_logger.LogError(request, ex.Message);
				return Result.Fail(ex.Message);
			}
		}
	}
}
