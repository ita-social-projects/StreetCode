using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.MediatR.Partner.GetById;

public record GetPartnerByIdQuery(int id) : IRequest<Result<PartnerDTO>>;
