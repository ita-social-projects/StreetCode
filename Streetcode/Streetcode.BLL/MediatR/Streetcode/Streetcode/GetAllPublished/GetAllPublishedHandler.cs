using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetAllPublished;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllCatalog
{
    public class GetAllPublishedHandler : IRequestHandler<GetAllPublishedQuery,
          Result<IEnumerable<StreetcodeShortDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetAllPublishedHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
          _repositoryWrapper = repositoryWrapper;
          _mapper = mapper;
        }

        public async Task<Result<IEnumerable<StreetcodeShortDto>>> Handle(GetAllPublishedQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
                predicate: sc => sc.Status == DAL.Enums.StreetcodeStatus.Published);

            if (streetcodes is null)
            {
                // Logger
                return Result.Fail("No streetcodes exist now");
            }

            return Result.Ok(_mapper.Map<IEnumerable<StreetcodeShortDto>>(streetcodes));
        }
   }
}
