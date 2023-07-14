using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.WithUrlExist
{
    public class StreetcodeWithUrlExistHandler : IRequestHandler<StreetcodeWithUrlExistQuery, Result<bool>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;

        public StreetcodeWithUrlExistHandler(IRepositoryWrapper repository, ILoggerService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(StreetcodeWithUrlExistQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repository.StreetcodeRepository.GetFirstOrDefaultAsync(predicate: st => st.TransliterationUrl == request.url);
            return Result.Ok(streetcodes != null);
        }
    }
}
