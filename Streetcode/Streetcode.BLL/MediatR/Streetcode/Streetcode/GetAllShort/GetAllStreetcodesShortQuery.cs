using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllShort;

public record GetAllStreetcodesShortQuery(ushort? page = null, ushort? pageSize = null)
    : IRequest<Result<GetAllStreetcodesShortDto>>;
