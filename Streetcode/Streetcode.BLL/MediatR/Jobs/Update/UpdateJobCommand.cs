using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.DTO.News;

namespace Streetcode.BLL.MediatR.Jobs.Update
{
	public record UpdateJobCommand(JobUpdateDto job)
		: IRequest<Result<JobDto>>;
}
