using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetAll
{
    public class GetAllCategoryNamesHandler : IRequestHandler<GetAllCategoryNamesQuery, Result<IEnumerable<CategoryWithNameDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

        public GetAllCategoryNamesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<IEnumerable<CategoryWithNameDTO>>> Handle(GetAllCategoryNamesQuery request, CancellationToken cancellationToken)
        {
            var allCategories = await _repositoryWrapper.SourceCategoryRepository.GetAllAsync();

            if (allCategories == null)
            {
                return Result.Fail(new Error(_stringLocalizerNo["NoCategories"].Value));
            }

            return Result.Ok(_mapper.Map<IEnumerable<CategoryWithNameDTO>>(allCategories));
        }
    }
}
