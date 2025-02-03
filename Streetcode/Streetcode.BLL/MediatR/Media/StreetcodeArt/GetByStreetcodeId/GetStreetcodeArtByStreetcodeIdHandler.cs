using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.StreetcodeArt.GetByStreetcodeId
{
  public class GetStreetcodeArtByStreetcodeIdHandler : IRequestHandler<GetStreetcodeArtByStreetcodeIdQuery, Result<IEnumerable<StreetcodeArtDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBlobService _blobService;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetStreetcodeArtByStreetcodeIdHandler(
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

        public async Task<Result<IEnumerable<StreetcodeArtDto>>> Handle(GetStreetcodeArtByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var streetcodeArts = await _repositoryWrapper
                .StreetcodeArtRepository
                .GetAllAsync(
                    predicate: sArt => sArt.StreetcodeId == request.StreetcodeId,
                    include: sArt => sArt
                        .Include(a => a.Art)
                        .Include(i => i.Art!.Image) !);

            if (streetcodeArts is null || request.StreetcodeId < 1)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyArtWithCorrespondingStreetcodeId", request.StreetcodeId].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var artsDto = _mapper.Map<IEnumerable<StreetcodeArtDto>>(streetcodeArts);

            foreach (var artDto in artsDto)
            {
                if (artDto.Art.Image != null)
                {
                    artDto.Art.Image.Base64 = _blobService.FindFileInStorageAsBase64(artDto.Art.Image.BlobName);
                }
            }

            return Result.Ok(artsDto);
        }
    }
}