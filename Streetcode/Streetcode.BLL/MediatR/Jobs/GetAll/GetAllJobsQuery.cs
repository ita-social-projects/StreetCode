using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;

namespace Streetcode.BLL.MediatR.Jobs.GetAll
{
	public record GetAllJobsQuery()
		: IRequest<Result<IEnumerable<JobDto>>>;
}
