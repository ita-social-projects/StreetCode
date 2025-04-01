using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Authentication.ValidateToken;

namespace Streetcode.BLL.MediatR.Authentication.ValidateToken
{
    public record ValidateTokenCommand(ValidateTokenDto ValidateTokenDto) : IRequest<Result<bool>>;
}