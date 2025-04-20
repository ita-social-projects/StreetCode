using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetById;

public record GetFactByIdQuery(int Id, UserRole? UserRole)
    : IRequest<Result<FactDto>>;
