using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Jobs.GetAll
{
    internal class GetAllShortJobsHandler
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
			Expression<Func<Job, bool>>? basePredicate = null;
			var predicate = basePredicate.ExtendWithAccessPredicate(new JobAccessManager(), request.UserRole);

			var jobs = await _repositoryWrapper.JobRepository.GetAllAsync(predicate: predicate);

			var jobsDto = _mapper.Map<IEnumerable<JobShortDto>>(jobs);
			return Result.Ok(jobsDto);
		}
	}
}
