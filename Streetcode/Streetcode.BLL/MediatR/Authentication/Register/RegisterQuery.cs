using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Authentication.Register
{
    public record RegisterQuery(RegisterRequestDTO registerRequestDTO)
        : IRequest<Result<RegisterResponseDTO>>;
}
