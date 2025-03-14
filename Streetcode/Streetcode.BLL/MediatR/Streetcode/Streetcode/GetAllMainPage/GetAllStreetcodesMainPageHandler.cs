﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllMainPage;

public class GetAllStreetcodesMainPageHandler : IRequestHandler<GetAllStreetcodesMainPageQuery,
    Result<IEnumerable<StreetcodeMainPageDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

    public GetAllStreetcodesMainPageHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService logger,
        IStringLocalizer<NoSharedResource> stringLocalizerNo)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerNo = stringLocalizerNo;
    }

    public async Task<Result<IEnumerable<StreetcodeMainPageDTO>>> Handle(GetAllStreetcodesMainPageQuery request, CancellationToken cancellationToken)
    {
        var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
            predicate: sc => sc.Status == StreetcodeStatus.Published,
            include: src => src
                .Include(item => item.Text)
                .Include(item => item.Images)
                .ThenInclude(x => x.ImageDetails!));

        var streetcodesList = streetcodes.ToList();
        if (streetcodesList.Any())
        {
            const int keyNumOfImageToDisplay = (int)ImageAssigment.Blackandwhite;
            foreach (var streetcode in streetcodesList)
            {
                streetcode.Images = streetcode.Images
                    .Where(x => x.ImageDetails != null && x.ImageDetails.Alt!.Equals(keyNumOfImageToDisplay.ToString()))
                    .ToList();
            }

            var random = new Random();
            var shuffledStreetcodes = streetcodesList.OrderBy(_ => random.Next());

            return Result.Ok(_mapper.Map<IEnumerable<StreetcodeMainPageDTO>>(shuffledStreetcodes));
        }

        var errorMsg = _stringLocalizerNo["NoStreetcodesExistNow"].Value;
        _logger.LogError(request, errorMsg);
        return Result.Fail(errorMsg);
    }
}