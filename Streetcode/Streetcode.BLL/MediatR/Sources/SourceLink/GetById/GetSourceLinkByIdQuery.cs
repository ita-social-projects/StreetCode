using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetById;

public record GetSourceLinkByIdQuery(int Id) : IRequest<Result<SourceLinkDTO>>;
