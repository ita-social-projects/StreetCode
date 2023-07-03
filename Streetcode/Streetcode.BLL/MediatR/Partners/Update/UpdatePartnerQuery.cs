using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.MediatR.Partners.Update
{
    public record UpdatePartnerQuery(CreatePartnerDTO Partner) : IRequest<Result<PartnerDTO>>;
}
