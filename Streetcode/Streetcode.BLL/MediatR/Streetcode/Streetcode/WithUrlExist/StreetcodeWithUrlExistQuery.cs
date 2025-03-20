using FluentResults;
using MediatR;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.WithUrlExist;

public record StreetcodeWithUrlExistQuery(string Url, UserRole? UserRole)
    : IRequest<Result<bool>>;