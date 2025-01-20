using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllFavourites
{
    public class GetAllStreetcodeFavouritesHandler : IRequestHandler<GetAllStreetcodeFavouritesQuery,
        Result<IEnumerable<StreetcodeFavouriteDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetAllStreetcodeFavouritesHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<StreetcodeFavouriteDTO>>> Handle(GetAllStreetcodeFavouritesQuery request, CancellationToken cancellationToken)
        {
            var favourites = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
                    predicate: fv => fv.Users.Any(u => u.Id == request.userId),
                    include: fv => fv.Include(item => item.Images).ThenInclude(x => x.ImageDetails!));
            if (favourites.Any())
            {
                const int keyNumOfImageToDisplay = (int)ImageAssigment.Blackandwhite;
                foreach (var streetcode in favourites)
                {
                    streetcode.Images = streetcode.Images.Where(x => x.ImageDetails != null && x.ImageDetails.Alt!.Equals(keyNumOfImageToDisplay.ToString())).ToList();
                }

                return Result.Ok(_mapper.Map<IEnumerable<StreetcodeFavouriteDTO>>(favourites).Where(s => request.type == null || s.Type == request.type));
            }

            string errorMsg = "NoFavouritesFound";
            _logger.LogError(request, errorMsg);
            return Result.Ok(Enumerable.Empty<StreetcodeFavouriteDTO>());
        }
    }
}
