using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;

namespace Streetcode.BLL.MediatR.Jobs.GetActiveJobs
{
	public record GetActiveJobsQuery : IRequest<Result<IEnumerable<JobDto>>>;
}
