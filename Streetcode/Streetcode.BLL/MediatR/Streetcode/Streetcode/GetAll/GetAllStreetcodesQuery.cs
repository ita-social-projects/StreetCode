using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;

public record GetAllStreetcodesQuery(
    int Page,
    int Amount,
    string? Title,
    string? Sort,
    string? Filter
    ) : IRequest<Result<IEnumerable<StreetcodeDTO>>>;