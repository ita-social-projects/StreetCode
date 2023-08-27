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
  public class GetPageOfArtsByStreetcodeIdHandler : IRequestHandler<GetPageOfArtsByStreetcodeIdQuery, Result<IEnumerable<ArtDTO>>>
    {
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetPageOfArtsByStreetcodeIdHandler(
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

        public async Task<Result<IEnumerable<ArtDTO>>> Handle(GetPageOfArtsByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var query = _repositoryWrapper.ArtRepository
                .FindAll(
                    predicate: sc => sc.StreetcodeArts.Any(s => s.StreetcodeId == request.StreetcodeId),
                    include: scl => scl.Include(sc => sc.Image))
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

            var arts = await query.ToListAsync(cancellationToken);

            var artsDto = _mapper.Map<IEnumerable<ArtDTO>>(arts);
            artsDto = ConvertArtImagesToBase64(artsDto);

            return Result.Ok(artsDto);
        }

        private IEnumerable<ArtDTO> ConvertArtImagesToBase64(IEnumerable<ArtDTO> artsDto)
        {
            foreach (var artDto in artsDto)
            {
                if (artDto.Image != null && artDto.Image.BlobName != null)
                {
                    artDto.Image.Base64 = _blobService.FindFileInStorageAsBase64(artDto.Image.BlobName);
                }
            }

            return artsDto;
        }
    }
}
