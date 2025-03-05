using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Users.GetByEmail;

public record GetByEmailQuery : IRequest<Result<UserDTO>>;