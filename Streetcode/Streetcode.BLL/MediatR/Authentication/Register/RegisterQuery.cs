using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Authentication.Register;

namespace Streetcode.BLL.MediatR.Authentication.Register
{
    public record RegisterQuery(RegisterRequestDTO registerRequestDTO)
        : IRequest<Result<RegisterResponseDTO>>;
}
