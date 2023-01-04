using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;

public record GetAllStreetcodesQuery : IRequest<Result<IEnumerable<StreetcodeDTO>>>;