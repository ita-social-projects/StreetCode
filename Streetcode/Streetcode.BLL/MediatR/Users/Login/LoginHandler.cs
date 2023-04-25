using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Users;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, Result<LoginResultDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ITokenService _tokenService;

        public LoginHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ITokenService tokenService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<Result<LoginResultDTO>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _repositoryWrapper.UserRepository
                .GetFirstOrDefaultAsync(u => u.Password == request.UserLogin.Password && u.Login == request.UserLogin.Login);
            if (user != null)
            {
                var token = _tokenService.GenerateJWTToken(user);
                var stringToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Result.Ok(new LoginResultDTO()
                {
                    User = _mapper.Map<UserDTO>(user),
                    Token = stringToken,
                    ExpireAt = token.ValidTo
                });
            }

            return Result.Fail("User not found");
        }
    }
}
