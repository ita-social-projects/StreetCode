using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetCount
{
    public record GetStreetcodesCountQuery(bool onlyPublished) : IRequest<Result<int>>;
}
