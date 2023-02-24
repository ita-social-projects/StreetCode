using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId
{
    public class GetArtsByStreetcodeIdHandler : IRequestHandler<GetArtsByStreetcodeIdQuery, Result<IEnumerable<ArtDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetArtsByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ArtDTO>>> Handle(GetArtsByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            if ((await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(s => s.Id == request.StreetcodeId)) is null)
            {
                return Result.Fail(
                    new Error($"Cannot find a arts by a streetcode id: {request.StreetcodeId}, because such streetcode doesn`t exist"));
            }

            var arts = await _repositoryWrapper.ArtRepository
               .GetAllAsync(
                   predicate: f => f.StreetcodeArts.Any(s => s.StreetcodeId == request.StreetcodeId),
                   include: art => art.Include(a => a.StreetcodeArts)
                   .ThenInclude(s => s.Streetcode));

            if (arts is null)
            {
                return Result.Fail(new Error($"Cannot find any art with corresponding streetcode id: {request.StreetcodeId}"));
            }

            var artDto = _mapper.Map<IEnumerable<ArtDTO>>(arts);
            return Result.Ok(artDto);
        }
    }
}
