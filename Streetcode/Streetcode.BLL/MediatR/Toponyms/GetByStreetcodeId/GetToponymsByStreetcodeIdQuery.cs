using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.BLL.MediatR.Toponyms.GetByStreetcodeId;

public record GetToponymsByStreetcodeIdQuery(int streetcodeId) : IRequest<Result<IEnumerable<ToponymDTO>>>;