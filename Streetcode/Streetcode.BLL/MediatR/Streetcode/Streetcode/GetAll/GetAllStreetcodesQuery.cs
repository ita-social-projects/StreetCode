using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;

public record GetAllStreetcodesQuery : IRequest<Result<IEnumerable<GeneralStreetodeDTO>>>;