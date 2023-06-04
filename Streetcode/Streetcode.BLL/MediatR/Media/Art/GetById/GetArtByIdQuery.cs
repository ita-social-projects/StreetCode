using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Art;

namespace Streetcode.BLL.MediatR.Media.Art.GetById;

public record GetArtByIdQuery(int Id) : IRequest<Result<ArtDTO>>;
