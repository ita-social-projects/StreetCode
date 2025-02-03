using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Users.GetOtherUserByUserName;

public record GetOtherUserByUserNameQuery(string UserName) : IRequest<Result<UserProfileDTO>>;