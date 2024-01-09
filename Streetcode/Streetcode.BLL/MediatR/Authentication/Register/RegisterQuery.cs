using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Authentication;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Users.SignUp
{
    public record RegisterQuery(RegisterRequestDTO registerRequestDTO) : IRequest<Result<RegisterResponseDTO>>;
}
