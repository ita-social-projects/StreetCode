using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Jobs.Update
{
    public class UpdateJobHandler : IRequestHandler<UpdateJobCommand, Result<JobDto>>
	{
		private readonly IRepositoryWrapper _repositoryWrapper;
		private readonly IMapper _mapper;
		private readonly ILoggerService _logger;
		private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizer;
		public UpdateJobHandler(IRepositoryWrapper repository, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizer)
		{
			_repositoryWrapper = repository;
			_mapper = mapper;
			_logger = logger;
			_stringLocalizer = stringLocalizer;
		}

		public async Task<Result<JobDto>> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
		{
			var existedJob =
				await _repositoryWrapper.JobRepository.GetFirstOrDefaultAsync(x => x.Id == request.job.Id);
			if (existedJob is null)
			{
				string exMessage = _stringLocalizer["CannotFindJobWithCorrespondingId", request.job.Id];
				_logger.LogError(request, exMessage);
				return Result.Fail(exMessage);
			}

			try
			{
				var jobToUpdate = _mapper.Map<Job>(request.job);
				_repositoryWrapper.JobRepository.Update(jobToUpdate);
				await _repositoryWrapper.SaveChangesAsync();
				return Result.Ok(_mapper.Map<JobDto>(jobToUpdate));
			}
			catch(Exception ex)
			{
				_logger.LogError(request, ex.Message);
				return Result.Fail(ex.Message);
			}
		}
	}
}
