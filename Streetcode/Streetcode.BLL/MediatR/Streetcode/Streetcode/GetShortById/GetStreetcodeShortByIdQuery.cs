using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetShortById;

public record GetStreetcodeShortByIdQuery(int Id, UserRole? UserRole)
    : IRequest<Result<StreetcodeShortDTO>>;