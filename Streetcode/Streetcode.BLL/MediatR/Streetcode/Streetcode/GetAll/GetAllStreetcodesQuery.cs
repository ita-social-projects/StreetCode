using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;

public record GetAllStreetcodesQuery(GetAllStreetcodesRequestDTO Request, UserRole? UserRole)
    : IRequest<Result<GetAllStreetcodesResponseDTO>>;