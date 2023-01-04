using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.MediatR.Media.Art.GetById;

public record GetArtByIdQuery(int Id) : IRequest<Result<ArtDTO>>;
