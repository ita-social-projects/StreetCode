using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Partners.Update;

namespace Streetcode.BLL.MediatR.Partners.Update
{
  public record UpdatePartnerQuery(UpdatePartnerDTO Partner)
        : IRequest<Result<PartnerDto>>;
}
