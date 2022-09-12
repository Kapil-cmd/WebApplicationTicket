using Microsoft.Data.SqlClient;
using Repository;

namespace Web.StaticModel
{
    public static class PermissionChecker
    {
        public static  bool HasPermission(string username, string permissionValue,TicketingContext context)
        {
            var Id = context.Users.FirstOrDefault(x => username == x.UserName)?.Id;
            var permissions = context.Permissions.FirstOrDefault(x => x.Slug == permissionValue);

            var users = context.Users;
            var rolePermissions = context.RolePermissions;
            var userRoles = context.UserRoles;

            var hasPermission = (from rp in rolePermissions
                                 join ur in userRoles on rp.RoleId equals ur.RoleId
                                 where ur.UserId == Id && rp.PermissionId == permissions.PermissionId
                                 select rp
                                ).Any();

            if(hasPermission == true)
            {
                return true; 
            }
            else
            {
                return false;
            }
            
        }

    }
}



