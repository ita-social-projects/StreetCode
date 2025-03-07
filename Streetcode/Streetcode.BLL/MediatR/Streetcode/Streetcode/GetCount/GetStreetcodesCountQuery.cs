using FluentResults;
using MediatR;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetCount;

public record GetStreetcodesCountQuery(bool OnlyPublished, UserRole? UserRole)
    : IRequest<Result<int>>;