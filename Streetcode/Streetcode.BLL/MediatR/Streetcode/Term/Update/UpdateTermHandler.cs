using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Update
{
    public class UpdateTermHandler : IRequestHandler<UpdateTermCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvertNull;
        private readonly IStringLocalizer<FailedToUpdateSharedResource> _stringLocalizerFailedToUpdate;

        public UpdateTermHandler(IMapper mapper, IRepositoryWrapper repository, IStringLocalizer<FailedToUpdateSharedResource> stringLocalizerFailedToUpdate, IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvertNull)
        {
            _mapper = mapper;
            _repository = repository;
            _stringLocalizerFailedToUpdate = stringLocalizerFailedToUpdate;
            _stringLocalizerCannotConvertNull = stringLocalizerCannotConvertNull;
        }

        public async Task<Result<Unit>> Handle(UpdateTermCommand request, CancellationToken cancellationToken)
        {
            var term = _mapper.Map<DAL.Entities.Streetcode.TextContent.Term>(request.Term);
            if (term is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotConvertNull["CannotConvertNullToTerm"].Value));
            }

            _repository.TermRepository.Update(term);

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;
            return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerFailedToUpdate["FailedToUpdateTerm"].Value));
        }
    }
}
