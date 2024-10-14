using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;

namespace Streetcode.BLL.MediatR.Jobs.Update
{
    public record UpdateJobCommand(JobUpdateDto job)
		: IRequest<Result<JobDto>>;
}
