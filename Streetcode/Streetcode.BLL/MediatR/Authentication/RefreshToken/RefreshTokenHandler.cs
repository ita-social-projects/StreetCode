using System.IdentityModel.Tokens.Jwt;
using FluentResults;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Streetcode.BLL.DTO.Authentication.RefreshToken;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Users;
namespace Streetcode.BLL.MediatR.Authentication.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenQuery, Result<RefreshTokenResponceDTO>>
    {
        private readonly ITokenService _tokenService;
        private readonly ILoggerService _logger;

        public RefreshTokenHandler(ITokenService tokenService, ILoggerService logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Result<RefreshTokenResponceDTO>> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            JwtSecurityToken? token = null;
            try
            {
                token = _tokenService.RefreshToken(request.token.Token);
            }
            catch (SecurityTokenValidationException ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail("error");
            }

            return new RefreshTokenResponceDTO() { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpireAt = token!.ValidTo };
        }
    }
}
