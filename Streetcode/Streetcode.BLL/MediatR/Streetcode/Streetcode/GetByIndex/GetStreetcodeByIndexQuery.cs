using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex;

public record GetStreetcodeByIndexQuery(int Index) : IRequest<Result<StreetcodeDTO>>;
