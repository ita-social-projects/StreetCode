using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.MediatR.Partners.Create
{
  public record CreatePartnerQuery(CreatePartnerDTO newPartner) : IRequest<Result<PartnerDTO>>;
}
