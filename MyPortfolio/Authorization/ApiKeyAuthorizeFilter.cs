using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace MyPortfolio.Authorization
{
    public class ApiKeyAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IConfiguration _configuration;
        private const string ApiKeyHeaderName = "X-Api-Key"; 

        public ApiKeyAuthorizeFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("API Key is missing.");
                return;
            }

            var apiKey = _configuration.GetValue<string>("ApiKey");

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("Invalid API Key.");
                return;
            }

            await Task.CompletedTask;
        }
    }
}