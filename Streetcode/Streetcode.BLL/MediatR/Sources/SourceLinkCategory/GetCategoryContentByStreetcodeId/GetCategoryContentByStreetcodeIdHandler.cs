using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetCategoryContentByStreetcodeId
{
    public class GetCategoryContentByStreetcodeIdHandler : IRequestHandler<GetCategoryContentByStreetcodeIdQuery, Result<StreetcodeCategoryContentDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetCategoryContentByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<StreetcodeCategoryContentDTO>> Handle(GetCategoryContentByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<StreetcodeContent, bool>>? basePredicate = str => str.Id == request.StreetcodeId;
            var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole);

            var isStreetcodeExists = await _repositoryWrapper.StreetcodeRepository.FindAll(predicate: predicate).AnyAsync(cancellationToken);

            if (!isStreetcodeExists)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyStreetcodeWithCorrespondingId", request.StreetcodeId].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var streetcodeContent = await _repositoryWrapper.StreetcodeCategoryContentRepository.GetFirstOrDefaultAsync(predicate: sc => sc.StreetcodeId == request.StreetcodeId && sc.SourceLinkCategoryId == request.CategoryId);

            if (streetcodeContent == null)
            {
                string errorMsg = _stringLocalizerNo["NoStreetcodeContentExist"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<StreetcodeCategoryContentDTO>(streetcodeContent));
        }
    }
}
