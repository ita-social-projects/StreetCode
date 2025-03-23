using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Util.Helpers;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetFavouriteById;

public class GetFavouriteByIdHandler : IRequestHandler<GetFavouriteByIdQuery,
    Result<StreetcodeFavouriteDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetFavouriteByIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerNo = stringLocalizerNo;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<StreetcodeFavouriteDto>> Handle(GetFavouriteByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = HttpContextHelper.GetCurrentUserId(_httpContextAccessor);

        var favourite = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(
            predicate: fv => fv.UserFavourites!.Any(u => u.Id == userId) && fv.Id == request.StreetcodeId,
            include: fv => fv.Include(item => item.Images).ThenInclude(x => x.ImageDetails!));

        if (favourite is not null)
        {
            const int keyNumOfImageToDisplay = (int)ImageAssigment.Blackandwhite;
            favourite.Images = favourite.Images.Where(x => x.ImageDetails != null && x.ImageDetails.Alt!.Equals(keyNumOfImageToDisplay.ToString())).ToList();
            return Result.Ok(_mapper.Map<StreetcodeFavouriteDto>(favourite));
        }

        string errorMsg = _stringLocalizerNo["NoFavouritesWithId", request.StreetcodeId].Value;
        _logger.LogError(request, errorMsg);
        return Result.Fail(errorMsg);
    }
}