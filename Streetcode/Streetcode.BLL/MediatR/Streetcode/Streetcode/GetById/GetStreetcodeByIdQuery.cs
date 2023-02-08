using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetById;

public record GetStreetcodeByIdQuery(int Id) : IRequest<Result<StreetcodeDTO>>;
