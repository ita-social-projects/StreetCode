using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.MediatR.Partners.GetAll;

public record GetAllPartnersQuery(ushort? page = null, ushort? pageSize = null)
    : IRequest<Result<GetAllPartnersDto>>;
