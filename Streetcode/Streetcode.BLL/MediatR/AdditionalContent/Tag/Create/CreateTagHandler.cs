using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Create
{
  public class CreateTagHandler : IRequestHandler<CreateTagCommand, Result<TagDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<FailedToValidateSharedResource> _stringLocalizerFailedToValidate;
        private readonly IStringLocalizer<FieldNamesSharedResource> _stringLocalizerFieldNames;

        public CreateTagHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILoggerService logger,
            IStringLocalizer<FailedToValidateSharedResource> stringLocalizerFailedToValidate,
            IStringLocalizer<FieldNamesSharedResource> stringLocalizerFieldNames)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerFailedToValidate = stringLocalizerFailedToValidate;
            _stringLocalizerFieldNames = stringLocalizerFieldNames;
        }

        public async Task<Result<TagDTO>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var exists = await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(t => request.tag.Title == t.Title);

            if (exists is not null)
            {
                var errMessage = _stringLocalizerFailedToValidate["MustBeUnique", _stringLocalizerFieldNames["Tag"]];
                _logger.LogError(request, errMessage);

                return Result.Fail(errMessage);
            }

            var newTag = await _repositoryWrapper.TagRepository.CreateAsync(new DAL.Entities.AdditionalContent.Tag()
            {
                Title = request.tag.Title
            });

            try
            {
                await _repositoryWrapper.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(request, ex.ToString());

                return Result.Fail(ex.ToString());
            }

            return Result.Ok(_mapper.Map<TagDTO>(newTag));
        }
    }
}
