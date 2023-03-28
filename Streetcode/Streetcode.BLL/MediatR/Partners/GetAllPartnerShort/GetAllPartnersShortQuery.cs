using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.MediatR.Partners.GetAllPartnerShort
{
    public record GetAllPartnersShortQuery : IRequest<Result<IEnumerable<PartnerShortDTO>>>;
}
