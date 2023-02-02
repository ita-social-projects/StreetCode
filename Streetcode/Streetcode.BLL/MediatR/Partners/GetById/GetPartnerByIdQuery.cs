using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.MediatR.Partners.GetById;

public record GetPartnerByIdQuery(int Id) : IRequest<Result<PartnerDTO>>;
