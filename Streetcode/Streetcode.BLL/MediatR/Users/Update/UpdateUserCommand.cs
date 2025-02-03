using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Users.Update;

public record UpdateUserCommand(UpdateUserDto UserDto) : IRequest<Result<UserDto>>;