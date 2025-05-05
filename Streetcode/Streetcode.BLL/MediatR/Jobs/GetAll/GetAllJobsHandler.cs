using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Jobs.GetAll
{
    public class GetAllJobsHandler
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
                Expression<Func<Job, bool>> basePredicate = t => true;
                var predicate = basePredicate.ExtendWithAccessPredicate(new JobAccessManager(), request.UserRole);

                if (predicate is null)
                {
                    predicate = basePredicate;
                }

                var allJobs = _repositoryWrapper
                    .JobRepository
                    .FindAll(predicate)
                    .ToList();

                var filteredJobs = string.IsNullOrWhiteSpace(request.title)
                    ? allJobs
                    : allJobs
                        .Where(p =>
                        {
                            if (string.IsNullOrWhiteSpace(p?.Title) || string.IsNullOrWhiteSpace(request.title))
                            {
                                return false;
                            }

                            string trimmedTitle = p.Title.Trim().ToLower();
                            string searchTitle = request.title.Trim().ToLower();
                            return trimmedTitle.Contains(searchTitle);
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
                    TotalAmount = filteredJobs.Count,
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
