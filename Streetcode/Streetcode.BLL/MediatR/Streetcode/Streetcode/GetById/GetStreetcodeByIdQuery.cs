using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetById;

public record GetStreetcodeByIdQuery(int Id, UserRole? userRole)
    : IRequest<Result<StreetcodeDTO>>;
