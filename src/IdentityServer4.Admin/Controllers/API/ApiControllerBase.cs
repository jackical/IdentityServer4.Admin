using System.Linq;
using IdentityServer4.Admin.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Admin.Controllers.API
{
    public class ApiControllerBase : Controller
    {
        protected ILogger Logger { get; }

        protected ApiControllerBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<ApiControllerBase>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!ModelState.IsValid)
            {
                context.Result = new ApiResult(ApiResultType.Forbidden,
                    ModelState.First(kv => kv.Value.ValidationState == ModelValidationState.Invalid).Value.Errors
                        .First().ErrorMessage);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}