using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Jobs.GetById
{
    public record GetJobByIdQuery(int JobId, UserRole? UserRole)
        : IRequest<Result<JobDto>>;
}
