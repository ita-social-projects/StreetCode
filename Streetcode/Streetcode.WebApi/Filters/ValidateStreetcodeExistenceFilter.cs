using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.WebApi.Filters
{
    public class ValidateStreetcodeExistenceFilter : IAsyncActionFilter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

        public ValidateStreetcodeExistenceFilter(IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("streetcodeId", out var streetcodeIdObj) && streetcodeIdObj is int streetcodeId)
            {
                var streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(s => s.Id == streetcodeId);
                if (streetcode is null)
                {
                    string errorMsg = _stringLocalizerNo["NoExistingStreetcodeWithId", streetcodeId].Value;
                    _logger.LogError(context.HttpContext.Request, errorMsg);
                    context.Result = new BadRequestObjectResult(new { Error = errorMsg });
                    return;
                }
            }

            await next();
        }
    }
}
