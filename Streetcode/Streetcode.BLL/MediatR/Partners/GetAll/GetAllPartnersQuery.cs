using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.MediatR.Partners.GetAll;

public record GetAllPartnersQuery : IRequest<Result<IEnumerable<PartnerDTO>>>;
