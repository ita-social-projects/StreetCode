using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;

namespace Streetcode.BLL.MediatR.Jobs.GetById
{
    public record GetJobByIdQuery(int JobId)
        : IRequest<Result<JobDto>>;
}
