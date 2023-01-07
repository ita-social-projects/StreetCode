using System.Collections;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetSubCategoriesByCategoryId;

public class GetSubCategoriesByCategoryIdHandler : IRequestHandler<GetSubCategoriesByCategoryIdQuery, Result<IEnumerable<SourceLinkSubCategoryDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetSubCategoriesByCategoryIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<SourceLinkSubCategoryDTO>>> Handle(GetSubCategoriesByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var sourceLink = await _repositoryWrapper
            .SourceSubCategoryRepository
            .GetAllAsync(
                predicate: ssc => ssc.SourceLinkCategoryId == request.categoryId,
                include: sscl => sscl
                    .Include(ssc => ssc.SourceLinks));

        if (sourceLink is null)
        {
            return Result.Fail(new Error($"Cannot find a sourceLink with corresponding categoryId: {request.categoryId}"));
        }

        var sourceLinkDto = _mapper.Map<IEnumerable<SourceLinkSubCategoryDTO>>(sourceLink);
        return Result.Ok(sourceLinkDto);
    }
}