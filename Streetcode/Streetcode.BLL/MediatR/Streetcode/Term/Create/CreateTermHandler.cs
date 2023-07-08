using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Create
{
    public class CreateTermHandler : IRequestHandler<CreateTermCommand, Result<TermDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvert;
        private readonly IStringLocalizer<CannotCreateSharedResource> _stringLocalizerCannotCreate;
        private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailedToCreate;

        public CreateTermHandler(
            IMapper mapper,
            IRepositoryWrapper repository,
            IStringLocalizer<CannotCreateSharedResource> stringLocalizerCannotCreate,
            IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailedToCreate,
            IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvert)
        {
            _mapper = mapper;
            _repository = repository;
            _stringLocalizerCannotCreate = stringLocalizerCannotCreate;
            _stringLocalizerFailedToCreate = stringLocalizerFailedToCreate;
            _stringLocalizerCannotConvert = stringLocalizerCannotConvert;
        }

        public async Task<Result<TermDTO>> Handle(CreateTermCommand request, CancellationToken cancellationToken)
        {
            var term = _mapper.Map<DAL.Entities.Streetcode.TextContent.Term>(request.Term);

            if (term is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotConvert["CannotConvertNullToTerm"].Value));
            }

            var createdTerm = _repository.TermRepository.Create(term);

            if (createdTerm is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotCreate["CannotCreateTerm"].Value));
            }

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;

            if(!resultIsSuccess)
            {
                return Result.Fail(new Error(_stringLocalizerFailedToCreate["FailedToCreateTerm"].Value));
            }

            var createdTermDTO = _mapper.Map<TermDTO>(createdTerm);

            return createdTermDTO != null ? Result.Ok(createdTermDTO) : Result.Fail(new Error(_stringLocalizerFailedToCreate["FailedToMapCreatedTerm"].Value));
        }
    }
}
