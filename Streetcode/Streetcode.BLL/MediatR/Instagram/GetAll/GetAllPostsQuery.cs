using FluentResults;
using MediatR;
using Streetcode.DAL.Entities.Instagram;

namespace Streetcode.BLL.MediatR.Instagram.GetAll;

public record GetAllPostsQuery : IRequest<Result<IEnumerable<InstagramPost>>>;