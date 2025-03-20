using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.Art.GetAll;

public record GetAllArtsQuery(UserRole? UserRole) : IRequest<Result<IEnumerable<ArtDTO>>>;