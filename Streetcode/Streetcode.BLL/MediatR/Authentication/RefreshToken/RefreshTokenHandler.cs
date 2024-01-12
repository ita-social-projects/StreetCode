using System.IdentityModel.Tokens.Jwt;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Authentication.RefreshToken;
using Streetcode.BLL.Interfaces.Users;
namespace Streetcode.BLL.MediatR.Authentication.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenQuery, Result<RefreshTokenResponceDTO>>
    {
        private readonly ITokenService _tokenService;

        public RefreshTokenHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<Result<RefreshTokenResponceDTO>> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            JwtSecurityToken? token = null;
            await Task.Run(() =>
            {
                token = _tokenService.RefreshToken(request.token.Token);
            });

            return new RefreshTokenResponceDTO() { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpireAt = token !.ValidTo };
        }
    }
}
