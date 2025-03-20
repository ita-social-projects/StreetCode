using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetAll
{
    public class GetAllCategoryNamesHandler : IRequestHandler<GetAllCategoryNamesQuery, Result<IEnumerable<CategoryWithNameDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

        public GetAllCategoryNamesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<IEnumerable<CategoryWithNameDTO>>> Handle(GetAllCategoryNamesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<DAL.Entities.Sources.SourceLinkCategory, bool>>? basePredicate = null;
            var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, sl => sl.Streetcodes);

            var allCategories = await _repositoryWrapper.SourceCategoryRepository.GetAllAsync(predicate: predicate);

            if (allCategories is null)
            {
                string errorMsg = _stringLocalizerNo["NoCategories"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<CategoryWithNameDTO>>(allCategories));
        }
    }
}
