using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Video.GetById;

public class GetVideoByIdHandler : IRequestHandler<GetVideoByIdQuery, Result<VideoDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetVideoByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<VideoDTO>> Handle(GetVideoByIdQuery request, CancellationToken cancellationToken)
    {
        var video = await _repositoryWrapper.VideoRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (video is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindVideoWithCorrespondingId", request.Id].Value));
        }

        var videoDto = _mapper.Map<VideoDTO>(video);
        return Result.Ok(videoDto);
    }
}