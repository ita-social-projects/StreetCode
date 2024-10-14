using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Jobs.GetAll
{
    internal class GetAllJobsHandler
		: IRequestHandler<GetAllJobsQuery, Result<IEnumerable<JobDto>>>
	{
		private readonly IRepositoryWrapper _repositoryWrapper;
		private readonly IMapper _mapper;
		private readonly ILoggerService _logger;

		public GetAllJobsHandler(IRepositoryWrapper repository, IMapper mapper, ILoggerService logger)
		{
			_repositoryWrapper = repository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<Result<IEnumerable<JobDto>>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var jobs = await _repositoryWrapper.JobRepository.GetAllAsync();
				var jobsDto = _mapper.Map<IEnumerable<JobDto>>(jobs);
				return Result.Ok(jobsDto);
			}
			catch(Exception ex)
			{
				_logger.LogError(request, ex.Message);
				return Result.Fail(ex.Message);
			}
		}
	}
}
