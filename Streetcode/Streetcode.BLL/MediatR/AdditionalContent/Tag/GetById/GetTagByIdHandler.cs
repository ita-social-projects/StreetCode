using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById;

public class GetTagByIdHandler : IRequestHandler<GetTagByIdQuery, Result<TagDTO>>
{
    private readonly ILoggerService _loggerService;
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTagByIdHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService loggerService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _loggerService = loggerService;
    }

    public async Task<Result<TagDTO>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        _loggerService.LogInformation("Entry into GetTagByIdHandler");

        var tag = await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (tag is null)
        {
            return Result.Fail(new Error($"Cannot find a Tag with corresponding id: {request.Id}"));
        }

        var tagDto = _mapper.Map<TagDTO>(tag);
        return Result.Ok(tagDto);
    }
}