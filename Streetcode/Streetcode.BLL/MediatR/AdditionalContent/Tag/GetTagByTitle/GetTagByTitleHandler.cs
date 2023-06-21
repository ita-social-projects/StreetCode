using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetTagByTitle;

public class GetTagByTitleHandler : IRequestHandler<GetTagByTitleQuery, Result<TagDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<GetTagByTitleHandler> _stringLocalizer;

    public GetTagByTitleHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<GetTagByTitleHandler> stringLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizer = stringLocalizer;
    }

    public async Task<Result<TagDTO>> Handle(GetTagByTitleQuery request, CancellationToken cancellationToken)
    {
        var tag = await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(f => f.Title == request.Title);

        if (tag is null)
        {
            return Result.Fail(new Error(_stringLocalizer?["CannotFindAnyTagByTitle", request.Title].Value));
        }

        var tagDto = _mapper.Map<TagDTO>(tag);
        return Result.Ok(tagDto);
    }
}
