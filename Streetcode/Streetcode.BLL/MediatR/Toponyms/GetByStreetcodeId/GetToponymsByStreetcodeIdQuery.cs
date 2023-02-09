using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.BLL.MediatR.Toponyms.GetByStreetcodeId;

public record GetToponymsByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<IEnumerable<ToponymDTO>>>;