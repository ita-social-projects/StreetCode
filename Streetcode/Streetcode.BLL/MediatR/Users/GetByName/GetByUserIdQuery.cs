using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Users.GetByName;

public record GetByUserIdQuery(string UserId) : IRequest<Result<UserDTO>>
{
    
}