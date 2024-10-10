using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.BlobStorage;
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
				PaginationResponse<Job> paginationResponse = _repositoryWrapper
					.JobRepository
					.GetAllPaginated(
						request.page,
						request.pageSize);

				GetAllJobsDTO getAllJobsDTO = new GetAllJobsDTO()
				{
					TotalAmount = paginationResponse.TotalItems,
					Jobs = _mapper.Map<IEnumerable<JobDto>>(paginationResponse.Entities),
				};

				return Task.FromResult(Result.Ok(getAllJobsDTO));
			}
			catch(Exception ex)
			{
				_logger.LogError(request, ex.Message);
				return Task.FromResult(Result.Fail<GetAllJobsDTO>(ex.Message));
			}
		}
	}
}
