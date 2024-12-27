using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Jobs.ChangeStatus
{
    public class ChangeJobStatusHandler :
		IRequestHandler<ChangeJobStatusCommand, Result<int>>
	{
		private readonly IMapper _mapper;
		private readonly IRepositoryWrapper _repositoryWrapper;
		private readonly ILoggerService _logger;
		private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizer;
		public ChangeJobStatusHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizer)
		{
			_mapper = mapper;
			_repositoryWrapper = repositoryWrapper;
			_logger = logger;
			_stringLocalizer = stringLocalizer;
		}

		public async Task<Result<int>> Handle(ChangeJobStatusCommand request, CancellationToken cancellationToken)
		{
			var job = await _repositoryWrapper.JobRepository.GetFirstOrDefaultAsync(j => j.Id == request.jobChangeStatusDto.Id);
			if (job is null)
			{
				string exMessage = _stringLocalizer["CannotFindJobWithCorrespondingId", request.jobChangeStatusDto.Id];
				_logger.LogError(request, exMessage);
				return Result.Fail(exMessage);
			}

			try
			{
				if (job.Status == request.jobChangeStatusDto.Status)
				{
					return Result.Ok(job.Id);
				}

				job.Status = request.jobChangeStatusDto.Status;
				_repositoryWrapper.JobRepository.Update(job);
				await _repositoryWrapper.SaveChangesAsync();
				return Result.Ok(job.Id);
			}
			catch(Exception ex)
			{
				_logger.LogError(request, ex.Message);
				return Result.Fail(ex.Message);
			}
		}
	}
}
