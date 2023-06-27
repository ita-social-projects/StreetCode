using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.MediatR.Partners.Delete
{
    public record DeletePartnerQuery(int id) : IRequest<Result<PartnerDTO>>;
}
