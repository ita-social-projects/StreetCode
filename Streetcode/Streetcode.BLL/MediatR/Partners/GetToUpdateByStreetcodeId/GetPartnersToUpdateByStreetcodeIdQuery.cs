using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.MediatR.Partners.GetByStreetcodeIdToUpdate;

public record class GetPartnersToUpdateByStreetcodeIdQuery(int StreetcodeId)
    : IRequest<Result<IEnumerable<PartnerDTO>>>;
