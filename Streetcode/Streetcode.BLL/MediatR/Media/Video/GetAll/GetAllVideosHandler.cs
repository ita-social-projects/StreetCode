﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Video.GetAll;

public class GetAllVideosHandler : IRequestHandler<GetAllVideosQuery, Result<IEnumerable<VideoDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllVideosHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<VideoDTO>>> Handle(GetAllVideosQuery request, CancellationToken cancellationToken)
    {
        var videos = await _repositoryWrapper.VideoRepository.GetAllAsync();

        if (videos is null)
        {
            return Result.Fail(new Error($"Cannot find any videos"));
        }

        var videoDtos = _mapper.Map<IEnumerable<VideoDTO>>(videos);
        return Result.Ok(videoDtos);
    }
}