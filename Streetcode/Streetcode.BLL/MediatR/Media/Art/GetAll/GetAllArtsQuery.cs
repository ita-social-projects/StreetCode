using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.MediatR.Media.Art.GetAll;

public record GetAllArtsQuery : IRequest<Result<IEnumerable<ArtDTO>>>;