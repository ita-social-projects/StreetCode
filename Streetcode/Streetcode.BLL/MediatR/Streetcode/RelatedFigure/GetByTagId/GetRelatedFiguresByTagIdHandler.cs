using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByTagId
{
  internal class GetRelatedFiguresByTagIdHandler : IRequestHandler<GetRelatedFiguresByTagIdQuery, Result<IEnumerable<RelatedFigureDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetRelatedFiguresByTagIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<IEnumerable<RelatedFigureDTO>>> Handle(GetRelatedFiguresByTagIdQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository
                .GetAllAsync(
                predicate: sc => sc.Status == DAL.Enums.StreetcodeStatus.Published &&
                  sc.Tags.Select(t => t.Id).Any(tag => tag == request.tagId),
                include: scl => scl
                    .Include(sc => sc.Images)
                    .Include(sc => sc.Tags));

            if (streetcodes is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindAnyFactWithCorrespondingId", request.tagId].Value));
            }

            var relatedFigureDTO = _mapper.Map<IEnumerable<RelatedFigureDTO>>(streetcodes);
            return Result.Ok(relatedFigureDTO);
        }
    }
}
