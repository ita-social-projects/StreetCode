using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById;

public class GetTagByIdHandler : IRequestHandler<GetTagByIdQuery, Result<TagDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<GetTagByIdHandler> _stringLocalizer;

    public GetTagByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<GetTagByIdHandler> stringLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizer = stringLocalizer;
    }

    public async Task<Result<TagDTO>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        var tag = await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (tag is null)
        {
            return Result.Fail(new Error(_stringLocalizer?["CannotFindTagWithCorrespondingId", request.Id].Value));
        }

        var tagDto = _mapper.Map<TagDTO>(tag);
        return Result.Ok(tagDto);
    }
}
