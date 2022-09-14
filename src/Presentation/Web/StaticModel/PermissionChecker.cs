using Repository;
using System.Security.Claims;

namespace Web.StaticModel
{
    public static class PermissionChecker
    {

        private static TicketingContext db;
        public static bool HasPermission(string username, string permissionValue)
        {
            var Id = db.Users.FirstOrDefault(x => username == x.UserName)?.UserName;
            var claims = ClaimsPrincipal.Current.Identities.First().Claims.First();

            var permissions = db.Permissions.FirstOrDefault(x => x.Slug == permissionValue);

            var users = db.Users;
            var rolePermissions = db.RolePermissions;
            var userRoles = db.UserRoles;

            var hasPermission = (from rp in rolePermissions
                                 join ur in userRoles on rp.RoleName equals ur.RoleName
                                 where ur.UserName == Id && rp.PermissionId == permissions.PermissionId
                                 select rp
                                ).Any();

            if (hasPermission == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

    }
}



