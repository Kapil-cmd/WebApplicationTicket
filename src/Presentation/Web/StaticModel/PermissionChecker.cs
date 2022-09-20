using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Repos.Work;
using System.Security.Claims;

namespace Web.StaticModel
{
    public static class PermissionChecker
    {
        public static IUnitOfWork unitOfWork;
        public static TicketingContext db;


        public static bool HasPermission(string username, string permissionValue)
        {
            var userName = db.Users.FirstOrDefault(x => x.UserName == unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name));
            userName = username;

            var permissionId = db.Permissions.Include(x => x.PermissionId).Select(x => new { x.PermissionId }).ToList();
           

            var users = db.Users;
            var rolePermissions = db.RolePermissions;
            var userRoles = db.UserRoles;
            var permission = db.Permissions;

            var hasPermission = (from rp in rolePermissions
                                 join ur in userRoles on rp.RoleName equals ur.RoleName
                                 where ur.UserName == username && rp.PermissionId == permissionValue
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



