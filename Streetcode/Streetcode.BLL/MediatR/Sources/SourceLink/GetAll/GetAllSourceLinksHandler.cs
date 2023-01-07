using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.MediatR.Media.Image.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetAll;

public class GetAllSourceLinksHandler : IRequestHandler<GetAllSourceLinksQuery, Result<IEnumerable<SourceLinkDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllSourceLinksHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<SourceLinkDTO>>> Handle(GetAllSourceLinksQuery request, CancellationToken cancellationToken)
    {
        var sourceLink = await _repositoryWrapper.SourceLinkRepository.GetAllAsync(
            include: s => s.Include(l => l.SubCategories));

        var sourceLinkDtos = _mapper.Map<IEnumerable<SourceLinkDTO>>(sourceLink);
        return Result.Ok(sourceLinkDtos);
    }
}