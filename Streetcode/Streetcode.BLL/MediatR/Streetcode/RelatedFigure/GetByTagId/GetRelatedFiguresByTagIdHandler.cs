using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByTagId
{
  internal class GetRelatedFiguresByTagIdHandler : IRequestHandler<GetRelatedFiguresByTagIdQuery, Result<IEnumerable<RelatedFigureDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetRelatedFiguresByTagIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<IEnumerable<RelatedFigureDTO>>> Handle(GetRelatedFiguresByTagIdQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository
                .GetAllAsync(
                predicate: sc => sc.Status == DAL.Enums.StreetcodeStatus.Published &&
                  sc.Tags.Select(t => t.Id).Any(tag => tag == request.tagId),
                include: scl => scl
                    .Include(sc => sc.Images).ThenInclude(x => x.ImageDetails)
                    .Include(sc => sc.Tags));

            if (streetcodes != null)
            {
                const int keyNumOfImageToDisplay = (int)ImageAssigment.Blackandwhite;
                foreach (var streetcode in streetcodes)
                {
                    streetcode.Images = streetcode.Images.Where(x => x.ImageDetails.Alt.Equals(keyNumOfImageToDisplay.ToString())).ToList();
                }

                return Result.Ok(_mapper.Map<IEnumerable<RelatedFigureDTO>>(streetcodes));
            }

            string errorMsg = _stringLocalizerCannotFind["CannotFindAnyFactWithCorrespondingId", request.tagId].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}
