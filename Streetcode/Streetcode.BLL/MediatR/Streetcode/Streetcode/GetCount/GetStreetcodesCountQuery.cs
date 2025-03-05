using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetCount;

public record GetStreetcodesCountQuery(bool OnlyPublished)
    : IRequest<Result<int>>;