using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CalculatorApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthFilterAttribute : Attribute, IAuthorizationFilter

    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuration.GetValue<string>($"ApiKey");
            var apiKeyReceived = context.HttpContext.Request.Headers["X-Api-Key"];
            if ( apiKey == null || apiKeyReceived.Count == 0)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if (!apiKey.Equals(apiKeyReceived[0]))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
