using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllCatalog
{
    public class GetAllStreetcodesCatalogHandler : IRequestHandler<GetAllStreetcodesCatalogQuery,
        Result<IEnumerable<RelatedFigureDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetAllStreetcodesCatalogHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<RelatedFigureDTO>>> Handle(GetAllStreetcodesCatalogQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
                predicate: sc => sc.Status == DAL.Enums.StreetcodeStatus.Published,
                include: src => src.Include(item => item.Tags).Include(item => item.Images));

            if (streetcodes != null)
            {
                var skipped = streetcodes.Skip((request.page - 1) * request.count).Take(request.count);
                return Result.Ok(_mapper.Map<IEnumerable<RelatedFigureDTO>>(skipped));
            }

            return Result.Fail("No streetcodes exist now");
        }
    }
}
