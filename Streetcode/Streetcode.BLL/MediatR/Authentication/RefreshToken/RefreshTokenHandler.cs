﻿using System.IdentityModel.Tokens.Jwt;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Authentication.RefreshToken;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Services.Authentication;
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

        public Task<Result<RefreshTokenResponceDTO>> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            JwtSecurityToken? token = null;
            try
            {
                token = _tokenService.RefreshToken(request.token.AccessToken, request.token.RefreshToken);
            }
            catch (Exception ex) when (ex.Message == TokenService.InvalidRefreshTokenErrorMessage)
            {
                _logger.LogError(request, ex.Message);
                return Task.FromResult(Result.Fail<RefreshTokenResponceDTO>("Unauthorized"));
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Task.FromResult(Result.Fail<RefreshTokenResponceDTO>(ex.Message));
            }

            return Task.FromResult(Result.Ok(new RefreshTokenResponceDTO()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            }));
        }
    }
}
