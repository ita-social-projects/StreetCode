using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.BLL.MediatR.Toponyms.GetAll;

public record GetAllToponymsQuery(GetAllToponymsRequestDTO request)
    : IRequest<Result<GetAllToponymsResponseDTO>>;