using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;

public class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, Result<IEnumerable<TagDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllTagsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TagDTO>>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repositoryWrapper.TagRepository.GetAllAsync();

        if (tags is null)
        {
            return Result.Fail(new Error($"Cannot find any tags"));
        }

        var tagDtos = _mapper.Map<IEnumerable<TagDTO>>(tags);
        return Result.Ok(tagDtos);
    }
}