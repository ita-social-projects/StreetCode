using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;

public class GetCategoriesByStreetcodeIdHandler : IRequestHandler<GetCategoriesByStreetcodeIdQuery, Result<IEnumerable<SourceLinkCategoryDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetCategoriesByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<SourceLinkCategoryDTO>>> Handle(GetCategoriesByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var srcCategories = await _repositoryWrapper
            .SourceCategoryRepository
            .GetAllAsync(
                predicate: sc => sc.Streetcodes.Any(s => s.Id == request.StreetcodeId),
                include: scl => scl
                    .Include(sc => sc.SubCategories)
                    .Include(sc => sc.Image) !);

        if (srcCategories is null)
        {
            return Result.Fail(new Error($"Cant find any source category with the streetcode id {request.StreetcodeId}"));
        }

        var mappedSrcCategories = _mapper.Map<IEnumerable<SourceLinkCategoryDTO>>(srcCategories);
        return Result.Ok(mappedSrcCategories);
    }
}