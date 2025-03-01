using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetAll;

public record GetAllTextsQuery(UserRole? UserRole) : IRequest<Result<IEnumerable<TextDTO>>>;