using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetByStreetcodeId
{
    public class GetSourceLinkByStreetcodeIdQueryHandler : IRequestHandler<GetSourceLinkByStreetcodeIdQuery, Result<IEnumerable<SourceLinkDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetSourceLinkByStreetcodeIdQueryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<SourceLinkDTO>>> Handle(GetSourceLinkByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var sourceLink = await _repositoryWrapper.SourceLinkRepository.GetAllAsync(sourceLink => sourceLink.StreetcodeId == request.streetcodeId);
            if (sourceLink == null)
            {
                return Result.Fail(new Error("Can`t find sourceLink with this StreetcodeId"));
            }

            var sourceLinkDto = _mapper.Map<IEnumerable<SourceLinkDTO>>(sourceLink);
            return Result.Ok(sourceLinkDto);
        }
    }
}
