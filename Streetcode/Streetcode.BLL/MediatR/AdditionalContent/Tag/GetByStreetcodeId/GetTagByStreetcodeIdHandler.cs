using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetByStreetcodeId;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;

public class GetTagByStreetcodeIdHandler : IRequestHandler<GetTagByStreetcodeIdQuery, Result<IEnumerable<TagDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTagByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TagDTO>>> Handle(GetTagByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var tag = await _repositoryWrapper.TagRepository
            .GetAllAsync(f => f.Streetcodes.Any(s => s.Id == request.StreetcodeId));

        if (tag is null)
        {
            return Result.Fail(new Error($"Cannot find a Tag by a streetcode categoryId: {request.StreetcodeId}"));
        }

        var tagDto = _mapper.Map<IEnumerable<TagDTO>>(tag);
        return Result.Ok(tagDto);
    }
}