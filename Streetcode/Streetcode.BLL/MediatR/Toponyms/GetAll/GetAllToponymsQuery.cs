using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Toponyms.GetAll;

public record GetAllToponymsQuery(GetAllToponymsRequestDTO Request, UserRole? UserRole)
    : IRequest<Result<GetAllToponymsResponseDTO>>;