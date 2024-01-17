using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.MediatR.Partners.GetByIsKeyPartner;

public record GetPartnersByIsKeyPartnerQuery(bool IsKeyPartner) : IRequest<Result<IEnumerable<PartnerDTO>>>;