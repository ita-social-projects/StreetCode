using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByTagId
{
  internal class GetRelatedFiguresByTagIdHandler : IRequestHandler<GetRelatedFiguresByTagIdQuery, Result<IEnumerable<RelatedFigureDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetRelatedFiguresByTagIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<RelatedFigureDTO>>> Handle(GetRelatedFiguresByTagIdQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository
                .GetAllAsync(
                predicate: sc => sc.Tags.Select(t => t.Id).Any(tag => tag == request.tagId),
                include: scl => scl
                    .Include(sc => sc.Images)
                    .Include(sc => sc.Tags));

            if (streetcodes is null)
            {
                return Result.Fail(new Error($"Cannot find any streetcode with corresponding tagid: {request.tagId}"));
            }

            var relatedFigureDTO = _mapper.Map<IEnumerable<RelatedFigureDTO>>(streetcodes);
            return Result.Ok(relatedFigureDTO);
        }
    }
}
