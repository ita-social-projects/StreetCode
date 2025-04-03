using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Partners.GetByStreetcodeIdToUpdate;

public record class GetPartnersToUpdateByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
    : IRequest<Result<IEnumerable<PartnerDTO>>>;
