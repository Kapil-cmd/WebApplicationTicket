//using Microsoft.AspNetCore.Authorization.Policy;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using System.Security.Claims;

//namespace Services.Middleware
//{
//    public class PermissionMiddleware 
//    {
//        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

//        private const string SCOPE_PERMISSION_TYPE = "";
//        private readonly RequestDelegate _requestDelegate;

//        public PermissionMiddleware(RequestDelegate requestDelegate)
//        {
//            _requestDelegate = requestDelegate;
//        }  
//        public async Task InvokeAsync(HttpContext context)
//        {
//            if(context.User.Identity is not null
//                &&(context.User.Identity.IsAuthenticated)
//            {
//                Claim scope = context.User.Claims.First(x => x.Type == SCOPE_PERMISSION_TYPE);
//                (context.User.Identity as ClaimsIdentity)?.RemoveClaim(scope);
//                context.User.AddIdentity(new ClaimsIdentity(new List<Claim>()
//                {
//                    new Claim(SCOPE_PERMISSION_TYPE,)
//                }));
//            }
//            await _requestDelegate(context);
//        }
//    }
//    public static class MyAuthMiddlewareExtensions
//    {
//        public static IApplicationBuilder MyAuthMiddleware(this IApplicationBuilder builder)
//        {
//            return builder.UseMiddleware<MyAuthMiddleware>();
//        }
//    }
//}
