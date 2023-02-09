using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.BLL.MediatR.Toponyms.GetById;

public record GetToponymByIdQuery(int Id) : IRequest<Result<ToponymDTO>>;
