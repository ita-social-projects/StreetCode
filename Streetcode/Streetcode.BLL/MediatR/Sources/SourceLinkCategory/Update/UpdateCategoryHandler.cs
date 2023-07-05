using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Streetcode.Fact.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Update
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvert;
        private readonly IStringLocalizer<FailedToUpdateSharedResource> _stringLocalizerFailedToUpdate;
        public UpdateCategoryHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IStringLocalizer<FailedToUpdateSharedResource> stringLocalizerFailedToUpdate,
            IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvert)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _stringLocalizerFailedToUpdate = stringLocalizerFailedToUpdate;
            _stringLocalizerCannotConvert = stringLocalizerCannotConvert;
        }

        public async Task<Result<Unit>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<DAL.Entities.Sources.SourceLinkCategory>(request.Category);
            if (category is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotConvert["CannotConvertNullToCategory"].Value));
            }

            _repositoryWrapper.SourceCategoryRepository.Update(category);

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerFailedToUpdate["FailedToUpdateCategory"].Value));
        }
    }
}
