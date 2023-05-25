using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Security.Claims;

namespace WebApp
{
    public class RoleAuthorizationFilter : ActionFilterAttribute, IAsyncAuthorizationFilter
    {
        public AuthorizationPolicy Policy { get; }
        string role;
        public RoleAuthorizationFilter(string role)
        {
            Policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            this.role = role;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Allow Anonymous skips all authorization
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            var roleClaim = context.HttpContext.User.Claims.FirstOrDefault(cl=>cl.Type == ClaimTypes.Role).Value;

            if (roleClaim != role)
            {
                
                context.Result = new ContentResult()
                {
                    Content = "Forbidden",
                    StatusCode = StatusCodes.Status403Forbidden,
                };
            }
        }
    }
}
