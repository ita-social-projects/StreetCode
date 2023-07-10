using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.MediatR.Partners.GetByStreetcodeId;

public record GetPartnersByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<IEnumerable<PartnerDTO>>>;
