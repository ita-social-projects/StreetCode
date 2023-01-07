using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetByStreetcodeId;

public class GetSourceLinkByStreetcodeIdQueryHandler : IRequestHandler<GetSourceLinkByStreetcodeIdQuery, Result<IEnumerable<SourceLinkDTO>>>
{
    private readonly IMapper _mapper;
    private readonly StreetcodeDbContext _context;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetSourceLinkByStreetcodeIdQueryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, StreetcodeDbContext context)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _context = context;
    }

    public async Task<Result<IEnumerable<SourceLinkDTO>>> Handle(GetSourceLinkByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var sourceLinks = await _repositoryWrapper.SourceLinkRepository
            .GetAllAsync(
                predicate: sourceLink => sourceLink.StreetcodeId == request.streetcodeId,
                include: s => s
                    .Include(l => l.SubCategories)
                    .ThenInclude(sc => sc.SourceLinkCategory!.Image)
                    .Include(l => l.SubCategories)
                    .ThenInclude(sc => sc.SourceLinkCategory) !);

        if (sourceLinks is null)
        {
            return Result.Fail(new Error($"Can`t find sourceLink with this StreetcodeId {request.streetcodeId}"));
        }

        var sourceLinkDto = _mapper.Map<IEnumerable<SourceLinkDTO>>(sourceLinks);
        return Result.Ok(sourceLinkDto);
    }
}