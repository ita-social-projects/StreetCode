using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId
{
    public class GetArtByStreetcodeIdQueryHandler : IRequestHandler<GetArtByStreetcodeIdQuery, Result<IEnumerable<ArtDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetArtByStreetcodeIdQueryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ArtDTO>>> Handle(GetArtByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var art = await _repositoryWrapper.ArtRepository.GetAllAsync(f => f.Streetcodes.Any(s => s.Id == request.streetcodeId));
            if (art == null)
            {
                return Result.Fail(new Error("Can`t find Art with this StreetcodeId"));
            }

            var artDto = _mapper.Map<IEnumerable<ArtDTO>>(art);
            return Result.Ok(artDto);
        }
    }
}
