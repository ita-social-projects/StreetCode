using Microsoft.AspNetCore.Mvc;
using Streetcode.WebApi.Filters;

namespace Streetcode.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateStreetcodeExistenceAttribute : TypeFilterAttribute
    {
        public ValidateStreetcodeExistenceAttribute()
            : base(typeof(ValidateStreetcodeExistenceFilter))
        {
        }
    }
}
