using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Jobs.GetAll;

internal class GetAllShortJobsHandler
	: IRequestHandler<GetAllShortJobsQuery, Result<IEnumerable<JobShortDto>>>
{
	private readonly IRepositoryWrapper _repositoryWrapper;
	private readonly IMapper _mapper;

	public GetAllShortJobsHandler(
		IRepositoryWrapper repository,
		IMapper mapper)
	{
		_repositoryWrapper = repository;
		_mapper = mapper;
	}

	public async Task<Result<IEnumerable<JobShortDto>>> Handle(GetAllShortJobsQuery request, CancellationToken cancellationToken)
	{
		var jobs = await _repositoryWrapper.JobRepository.GetAllAsync();
		var jobsDto = _mapper.Map<IEnumerable<JobShortDto>>(jobs);

		return Result.Ok(jobsDto);
	}
}