using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Toponyms.GetById;

public record GetToponymByIdQuery(int Id, UserRole? UserRole)
    : IRequest<Result<ToponymDTO>>;
