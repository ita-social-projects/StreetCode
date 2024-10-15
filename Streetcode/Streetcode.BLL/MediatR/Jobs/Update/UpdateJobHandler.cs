using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Jobs.Update
{
    public class UpdateJobHandler : IRequestHandler<UpdateJobCommand, Result<JobDto>>
	{
		private readonly IRepositoryWrapper _repositoryWrapper;
		private readonly IMapper _mapper;
		private readonly ILoggerService _logger;
		public UpdateJobHandler(IRepositoryWrapper repository, IMapper mapper, ILoggerService logger)
		{
			_repositoryWrapper = repository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<Result<JobDto>> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
		{
			var existedJob =
				await _repositoryWrapper.JobRepository.GetFirstOrDefaultAsync(x => x.Id == request.job.Id);
			if (existedJob is null)
			{
				string exMessage = $"No job found by entered Id - {request.job.Id}";
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
