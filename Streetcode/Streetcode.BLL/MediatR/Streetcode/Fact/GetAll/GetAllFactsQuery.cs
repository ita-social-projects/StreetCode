using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;

public record GetAllFactsQuery(UserRole? UserRole) : IRequest<Result<IEnumerable<FactDto>>>;