using System.Linq.Expressions;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManagerService;
using Streetcode.DAL.Entities.Streetcode;
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
            Expression<Func<StreetcodeContent, bool>>? basePredicate = st => st.TransliterationUrl == request.url;
            var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.userRole);

            var streetcodes = await _repository.StreetcodeRepository.GetFirstOrDefaultAsync(predicate: predicate);
            return Result.Ok(streetcodes != null);
        }
    }
}
