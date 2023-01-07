using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoryById;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Result<SourceLinkCategoryDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetCategoryByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<SourceLinkCategoryDTO>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var srcCategories = await _repositoryWrapper
            .SourceCategoryRepository
            .GetFirstOrDefaultAsync(
                predicate: sc => sc.Id == request.Id,
                include: scl => scl
                    .Include(sc => sc.SubCategories)
                    .Include(sc => sc.Image) !);

        var mappedSrcCategories = _mapper.Map<SourceLinkCategoryDTO>(srcCategories);
        return Result.Ok(mappedSrcCategories);
    }
}