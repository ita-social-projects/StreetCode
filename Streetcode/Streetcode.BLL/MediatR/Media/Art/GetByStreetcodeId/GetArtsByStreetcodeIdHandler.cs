using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.DTO.Media.Art;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId
{
    public class GetArtsByStreetcodeIdHandler : IRequestHandler<GetArtsByStreetcodeIdQuery, Result<IEnumerable<ArtDTO>>>
    {
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetArtsByStreetcodeIdHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IBlobService blobService,
            ILoggerService logger,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobService = blobService;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<IEnumerable<ArtDTO>>> Handle(GetArtsByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var arts = await _repositoryWrapper.ArtRepository
                .GetAllAsync(
                predicate: art => art.StreetcodeArts.Any(sArt => sArt.StreetcodeArtSlide!.StreetcodeId == request.StreetcodeId),
                include: scl => scl
                    .Include(sc => sc.Image) !);

            if (!arts.Any())
            {
                string message = "Returning empty enumerable of arts";
                _logger.LogInformation(message);
                return Result.Ok(Enumerable.Empty<ArtDTO>());
            }

            var imageIds = arts.Where(a => a.Image != null).Select(a => a.Image!.Id);

            var artsDto = _mapper.Map<IEnumerable<ArtDTO>>(arts);
            foreach (var artDto in artsDto)
            {
                if (artDto.Image != null && artDto.Image.BlobName != null)
                {
                    artDto.Image.Base64 = _blobService.FindFileInStorageAsBase64(artDto.Image.BlobName);
                }
            }

            return Result.Ok(_mapper.Map<IEnumerable<ArtDTO>>(artsDto));
        }
    }
}
