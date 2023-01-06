using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetAll;

public record GetAllSourceLinksQuery : IRequest<Result<IEnumerable<SourceLinkDTO>>>;