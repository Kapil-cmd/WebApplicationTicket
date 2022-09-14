﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace Services.CustomFilter
{
    public class PermissionFilter : TypeFilterAttribute
    {
        private string actionName = string.Empty;
        public PermissionFilter(string claimValue = null) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { claimValue};
        }
        public class ClaimRequirementFilter : IAsyncActionFilter
        {
            private readonly string _claim;
            public ClaimRequirementFilter(string claim)
            {
                _claim = claim;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                TicketingContext db = context.HttpContext.RequestServices.GetRequiredService<TicketingContext>();

                var distributor = (ControllerActionDescriptor)context.ActionDescriptor;
                var actionName = distributor.ActionName;
                var controllerName = distributor.ControllerName;

                var userName = db.Users.FirstOrDefault(x=>x.UserName == context.HttpContext.User.Identity.Name)?.UserName;
                var permissionSlug = _claim.Split('&');
                var permissions = db.Permissions.FirstOrDefault(x => x.Slug == permissionSlug.Last());

                if (permissions == null)
                {
                    context.Result = new UnauthorizedResult();
                    return ;
                }

                var users = db.Users;
                var userRole = db.UserRoles;
                var rolePermissions = db.RolePermissions;

                var hasPermission = (from rp in rolePermissions
                                     join ur in userRole on rp.RoleName equals ur.RoleName
                                     where ur.UserName == userName && rp.PermissionId == permissions.PermissionId
                                     select rp
                                    ).Any();
                if (hasPermission)
                {
                    await next();
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
        }
    }
}
