using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Jobs.GetAll
{
    public class GetAllShortJobsHandler
		: IRequestHandler<GetAllShortJobsQuery, Result<IEnumerable<JobShortDto>>>
	{
		private readonly IRepositoryWrapper _repositoryWrapper;
		private readonly IMapper _mapper;
		private readonly ILoggerService _logger;

		public GetAllShortJobsHandler(IRepositoryWrapper repository, IMapper mapper, ILoggerService logger)
		{
			_repositoryWrapper = repository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<Result<IEnumerable<JobShortDto>>> Handle(GetAllShortJobsQuery request, CancellationToken cancellationToken)
		{
			var jobs = await _repositoryWrapper.JobRepository.GetAllAsync();
			var jobsDto = _mapper.Map<IEnumerable<JobShortDto>>(jobs);
			return Result.Ok(jobsDto);
		}
	}
}
