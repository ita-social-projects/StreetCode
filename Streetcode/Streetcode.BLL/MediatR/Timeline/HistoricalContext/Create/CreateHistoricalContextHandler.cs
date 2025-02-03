using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.Create
{
    public class CreateHistoricalContextHandler : IRequestHandler<CreateHistoricalContextCommand, Result<HistoricalContextDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<FailedToValidateSharedResource> _stringLocalizerValidation;
        private readonly IStringLocalizer<FieldNamesSharedResource> _stringLocalizerFieldNames;

        public CreateHistoricalContextHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<FailedToValidateSharedResource> stringLocalizerValidation, IStringLocalizer<FieldNamesSharedResource> stringLocalizerFieldNames)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerValidation = stringLocalizerValidation;
            _stringLocalizerFieldNames = stringLocalizerFieldNames;
        }

        public async Task<Result<HistoricalContextDto>> Handle(CreateHistoricalContextCommand request, CancellationToken cancellationToken)
        {
            try
            {
                /* read comment at Streetcode.BLL.MediatR.Timeline.HistoricalContext.Update line 29 */
                var context = _mapper.Map<DAL.Entities.Timeline.HistoricalContext>(request.HistoricalContext);
                var checkIfContextExists = await _repositoryWrapper.HistoricalContextRepository.GetFirstOrDefaultAsync(j => j.Title == request.HistoricalContext.Title);

                if (checkIfContextExists is not null)
                {
                    string exceptionMessege = _stringLocalizerValidation["MustBeUnique", _stringLocalizerFieldNames["Historical context title"]];
                    _logger.LogError(request, exceptionMessege);
                    return Result.Fail(exceptionMessege);
                }

                var createdContext = await _repositoryWrapper.HistoricalContextRepository.CreateAsync(context);
                await _repositoryWrapper.SaveChangesAsync();
                return Result.Ok(_mapper.Map<HistoricalContextDto>(createdContext));
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}