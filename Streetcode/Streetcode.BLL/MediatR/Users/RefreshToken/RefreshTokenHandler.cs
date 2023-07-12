using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Users;
using Streetcode.BLL.Services.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenQuery, Result<RefreshTokenResponce>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ITokenService _tokenService;

        public RefreshTokenHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ITokenService tokenService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<Result<RefreshTokenResponce>> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            JwtSecurityToken token = null;
            await Task.Run(() =>
            {
                token = _tokenService.RefreshToken(request.token.Token);
            });

            return new RefreshTokenResponce() { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpireAt = token.ValidTo };
        }
    }
}
