using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.Art.GetById;

public record GetArtByIdQuery(int Id, UserRole? UserRole)
    : IRequest<Result<ArtDTO>>;
