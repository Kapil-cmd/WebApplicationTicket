using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Services.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _permissionName;
        public PermissionMiddleware(RequestDelegate next, string permissionName)
        {
            _next = next;
            _permissionName = permissionName;
        }
        public async Task Invoke(HttpContext httpContext,IAuthorizationService authorizationService)
        {
            AuthorizationResult authorizationResult =
                await authorizationService.AuthorizeAsync(httpContext.User, null, _permissionName);

            if (!authorizationResult.Succeeded)
            {
                await httpContext.ChallengeAsync();
                return;
            }
            await _next(httpContext);
        }
    }
}
