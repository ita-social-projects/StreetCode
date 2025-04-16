using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Jobs.GetAll
{
	internal class GetAllJobsHandler
		: IRequestHandler<GetAllJobsQuery, Result<GetAllJobsDTO>>
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

		public Task<Result<GetAllJobsDTO>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var allJobs = await _repositoryWrapper
					.JobRepository
					.FindAll()
					.ToListAsync(cancellationToken);

				var filteredJobs = string.IsNullOrWhiteSpace(request.title)
					? allJobs
					: allJobs
						.Where(p =>
						{
							_logger.LogInformation($"request.title: '{request.title}', p.Title: '{p.Title}'");
							string trimmedTitle = p.Title?.Trim().ToLower();
							string searchTitle = request.title?.Trim().ToLower();
							return !string.IsNullOrWhiteSpace(trimmedTitle) &&
								   trimmedTitle.Contains(searchTitle);
						})
						.ToList();

				int page = request.page ?? 1;
				int pageSize = request.pageSize ?? 10;

				var pagedJobs = filteredJobs
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToList();

				var getAllJobsDTO = new GetAllJobsDTO
				{
					TotalAmount = filteredJobs.Count(),
					Jobs = _mapper.Map<IEnumerable<JobDto>>(pagedJobs),
				};

				return Task.FromResult(Result.Ok(getAllJobsDTO));
			}
			catch (Exception ex)
			{
				_logger.LogError(request, ex.Message);
				return Task.FromResult(Result.Fail<GetAllJobsDTO>(ex.Message));
			}
		}
	}
}
