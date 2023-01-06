using AutoMapper;
using FluentResults;
using MediatR;

using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetById;

public class GetSourceLinkByIdHandler : IRequestHandler<GetSourceLinkByIdQuery, Result<SourceLinkDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetSourceLinkByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<SourceLinkDTO>> Handle(GetSourceLinkByIdQuery request, CancellationToken cancellationToken)
    {
        var sourceLink = await _repositoryWrapper.SourceLinkRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (sourceLink is null)
        {
            return Result.Fail(new Error($"Cannot find a sourceLink with corresponding Id: {request.Id}"));
        }

        var sourceLinkDto = _mapper.Map<SourceLinkDTO>(sourceLink);
        return Result.Ok(sourceLinkDto);
    }
}