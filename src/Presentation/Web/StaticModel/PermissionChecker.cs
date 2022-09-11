using Microsoft.AspNetCore.Mvc.Filters;
using Repository;

namespace Web.StaticModel
{
    public static class PermissionChecker
    {
           public static bool HasPermission(string username, string permissionValue, ActionExecutingContext context)
           {
              TicketingContext db = context.HttpContext.RequestServices.GetRequiredService<TicketingContext>();
              var Id = db.Users.FirstOrDefault(x => x.UserName == context.HttpContext.User.Identity.Name)?.Id;

              var user = db.Users;
              var userRoles = db.UserRoles;
              var rolePermissions = db.RolePermissions;
              var permission = db.Permissions;
              //var haspermission = (from rp in rolePermissions
              //                   join ur in userRoles on rp.RoleId equals ur.RoleId
              //                   where ur.UserId == Id && rp.PermissionId == permission.).Any();
            
            return true;
           }
    }
}

