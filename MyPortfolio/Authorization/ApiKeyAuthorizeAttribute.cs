using Microsoft.AspNetCore.Mvc;

namespace MyPortfolio.Authorization
{
    public class ApiKeyAuthorizeAttribute : TypeFilterAttribute
    {
        public ApiKeyAuthorizeAttribute() : base(typeof(ApiKeyAuthorizeFilter))
        {
        }
    }
}