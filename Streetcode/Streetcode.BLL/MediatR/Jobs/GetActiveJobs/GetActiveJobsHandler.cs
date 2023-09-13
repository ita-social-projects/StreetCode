using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Jobs.GetActiveJobs
{
	public class GetActiveJobsHandler : IRequestHandler<GetActiveJobsQuery, Result<IEnumerable<JobDto>>>
	{
		private readonly IRepositoryWrapper _repositoryWrapper;
		private readonly IMapper _mapper;
		private readonly ILoggerService _logger;
		public GetActiveJobsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
		{
			_repositoryWrapper = repositoryWrapper;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<Result<IEnumerable<JobDto>>> Handle(GetActiveJobsQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var jobs = await _repositoryWrapper.JobRepository.GetAllAsync(predicate: j => j.Status == true);
				var jobsDto = _mapper.Map<IEnumerable<JobDto>>(jobs);
				return Result.Ok(jobsDto);
			}
			catch (Exception ex)
			{
				_logger.LogError(request, ex.Message);
				return Result.Fail(ex.Message);
			}
		}
	}
}