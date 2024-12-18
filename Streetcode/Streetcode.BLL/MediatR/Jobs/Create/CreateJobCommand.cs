using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;

namespace Streetcode.BLL.MediatR.Jobs.Create
{
    public record CreateJobCommand(JobCreateDto job)
        : IRequest<Result<JobDto>>;
}
