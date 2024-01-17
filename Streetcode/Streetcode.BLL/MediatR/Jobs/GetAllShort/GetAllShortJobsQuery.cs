using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;

namespace Streetcode.BLL.MediatR.Jobs.GetAll
{
	public record GetAllShortJobsQuery()
		: IRequest<Result<IEnumerable<JobShortDto>>>;
}
