﻿using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByTagId
{
    internal class GetRelatedFiguresByTagIdHandler : IRequestHandler<GetRelatedFiguresByTagIdQuery, Result<IEnumerable<RelatedFigureDTO>?>>
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

        // If you use Rider instead of Visual Studio, for example, "SuppressMessage" attribute suppresses PossibleMultipleEnumeration warning
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Here is no sense to do materialization of query because of nested ToListAsync method in GetAllAsync method")]
        public async Task<Result<IEnumerable<RelatedFigureDTO>?>> Handle(GetRelatedFiguresByTagIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<StreetcodeContent, bool>>? basePredicate = sc => sc.Tags.Select(t => t.Id).Any(tag => tag == request.TagId);
            var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole);

            var streetcodes = await _repositoryWrapper.StreetcodeRepository
                .GetAllAsync(
                predicate: predicate,
                include: scl => scl
                    .Include(sc => sc.Images).ThenInclude(x => x.ImageDetails)
                    .Include(sc => sc.Tags));

            const int blackAndWhiteImageAssignmentKey = (int)ImageAssigment.Blackandwhite;
            foreach (var streetcode in streetcodes)
            {
                streetcode.Images = streetcode.Images.Where(x => x.ImageDetails != null && x.ImageDetails.Alt!.Equals(blackAndWhiteImageAssignmentKey.ToString())).ToList();
            }

            if (!streetcodes.Any())
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyFactWithCorrespondingId", request.TagId].Value;
                _logger.LogError(request, errorMsg);

                return Result.Ok<IEnumerable<RelatedFigureDTO>?>(null);
            }

            return Result.Ok<IEnumerable<RelatedFigureDTO>?>(_mapper.Map<IEnumerable<RelatedFigureDTO>>(streetcodes));
        }
    }
}
