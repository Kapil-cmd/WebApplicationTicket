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
            var Id = db.Users.FirstOrDefault(x => x.UserName == unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userName = unitOfWork._db.Users.FirstOrDefault(x => x.Id == x.Id);


            var claims = ClaimsPrincipal.Current.Identities.First().Claims.First();
            var permissions = db.Permissions.FirstOrDefault(x => x.Slug == permissionValue);
            
            var users = db.Users;
            var rolePermissions = db.RolePermissions;
            var userRoles = db.UserRoles;

            var hasPermission = (from rp in rolePermissions
                                 join ur in userRoles on rp.RoleName equals ur.RoleName
                                 where ur.UserName == username && rp.PermissionId == permissions.PermissionId
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



