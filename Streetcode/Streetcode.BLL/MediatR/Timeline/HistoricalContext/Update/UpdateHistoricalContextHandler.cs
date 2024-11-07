using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.Update
{
    public class
        UpdateHistoricalContextHandler : IRequestHandler<UpdateHistoricalContextCommand, Result<HistoricalContextDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<FailedToValidateSharedResource> _stringLocalizerValidation;
        private readonly IStringLocalizer<FieldNamesSharedResource> _stringLocalizerFieldNames;

        public UpdateHistoricalContextHandler(
            IRepositoryWrapper repository,
            IMapper mapper,
            ILoggerService logger,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind,
            IStringLocalizer<FailedToValidateSharedResource> stringLocalizerValidation,
            IStringLocalizer<FieldNamesSharedResource> stringLocalizerFieldNames)
        {
            _repositoryWrapper = repository;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
            _stringLocalizerValidation = stringLocalizerValidation;
            _stringLocalizerFieldNames = stringLocalizerFieldNames;
        }

        public async Task<Result<HistoricalContextDTO>> Handle(
            UpdateHistoricalContextCommand request,
            CancellationToken cancellationToken)
        {
            var historicalContext =
                await _repositoryWrapper.HistoricalContextRepository.GetFirstOrDefaultAsync(x =>
                    x.Id == request.HistoricalContext.Id);
            if (historicalContext is null)
            {
                string exMessage = _stringLocalizerCannotFind["CannotFindHistoricalContextWithCorrespondingId", request.HistoricalContext.Id];
                _logger.LogError(request, exMessage);
                return Result.Fail(exMessage);
            }

            var historicalContextRepeat = await _repositoryWrapper.HistoricalContextRepository.GetFirstOrDefaultAsync(
                x =>
                    x.Title == request.HistoricalContext.Title);

            if (historicalContextRepeat is not null)
            {
                string exMessage = _stringLocalizerValidation["MustBeUnique", _stringLocalizerFieldNames["Historical context title"]];
                _logger.LogError(request, exMessage);
                return Result.Fail(exMessage);
            }

            try
            {
                /* have some shitty code right here, if you delete the DAL.Entities.Timeline. part it will show error
                even if you have using Streetcode.DAL.Entities.Timeline; so sad :( */
                var contextToUpdate = _mapper.Map<DAL.Entities.Timeline.HistoricalContext>(request.HistoricalContext);
                _repositoryWrapper.HistoricalContextRepository.Update(contextToUpdate);
                await _repositoryWrapper.SaveChangesAsync();
                return Result.Ok(_mapper.Map<HistoricalContextDTO>(contextToUpdate));
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}