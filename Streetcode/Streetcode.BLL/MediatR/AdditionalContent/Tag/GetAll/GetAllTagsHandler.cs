using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;

public class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, Result<IEnumerable<TagDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<GetAllTagsHandler> _stringLocalizer;

    public GetAllTagsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<GetAllTagsHandler> stringLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizer = stringLocalizer;
    }

    public async Task<Result<IEnumerable<TagDTO>>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repositoryWrapper.TagRepository.GetAllAsync();

        if (tags is null)
        {
            return Result.Fail(_stringLocalizer?["CannotFindAnyTags"].Value);
        }

        var tagDtos = _mapper.Map<IEnumerable<TagDTO>>(tags);
        return Result.Ok(tagDtos);
    }
}
