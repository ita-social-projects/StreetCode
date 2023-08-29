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
  public class GetPageOfArtsByStreetcodeIdHandler : IRequestHandler<GetPageOfArtsByStreetcodeIdQuery, Result<IEnumerable<StreetcodeArtDTO>>>
    {
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetPageOfArtsByStreetcodeIdHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IBlobService blobService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<Result<IEnumerable<StreetcodeArtDTO>>> Handle(GetPageOfArtsByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var query = _repositoryWrapper.StreetcodeArtRepository
                .FindAll(
                    predicate: s => s.StreetcodeId == request.StreetcodeId,
                    include: art => art
                        .Include(a => a.Art)
                        .Include(i => i.Art.Image) !)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

            var arts = await query.ToListAsync(cancellationToken);

            var artsDto = _mapper.Map<IEnumerable<StreetcodeArtDTO>>(arts);
            artsDto = ConvertArtImagesToBase64(artsDto);

            return Result.Ok(artsDto);
        }

        private IEnumerable<StreetcodeArtDTO> ConvertArtImagesToBase64(IEnumerable<StreetcodeArtDTO> artsDto)
        {
            foreach (var artDto in artsDto)
            {
                if (artDto.Art.Image != null && artDto.Art.Image.BlobName != null)
                {
                    artDto.Art.Image.Base64 = _blobService.FindFileInStorageAsBase64(artDto.Art.Image.BlobName);
                }
            }

            return artsDto;
        }
    }
}
