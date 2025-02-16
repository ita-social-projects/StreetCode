using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.WithUrlExist;

public record StreetcodeWithUrlExistQuery(string Url)
    : IRequest<Result<bool>>;