using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Streetcode.BLL.DTO.Users;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, Result<LoginResultDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IConfiguration _configuration;

        public LoginHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IConfiguration configuration)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<Result<LoginResultDTO>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await GetUser(request.UserLogin.Login, request.UserLogin.Password);
            if (user != null)
            {
                var token = Generate(user);
                return Result.Ok(new LoginResultDTO() { User = _mapper.Map<UserDTO>(user), Token = token });
            }

            return Result.Fail("User not found");
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User> GetUser(string login, string password)
        {
            List<User> users = new List<User>
            {
                new User()
                {
                    Email = "email1@gmail.com",
                    Id = 1,
                    Login = "login1",
                    Name = "name1",
                    Password = "password1",
                    Role = DAL.Enums.UserRole.MainAdministrator,
                    Surname = "surname1"
                },
                new User()
                {
                    Email = "email2@gmail.com",
                    Id = 2,
                    Login = "login2",
                    Name = "name2",
                    Password = "password2",
                    Role = DAL.Enums.UserRole.Administrator,
                    Surname = "surname2"
                }
            };
            User founded = null;
            await Task.Run(() =>
            founded = users.FirstOrDefault(u => password.Equals(u.Password) && login.Equals(u.Login)));

            return founded;
        }
    }
}
