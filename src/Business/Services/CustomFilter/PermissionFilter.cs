using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Repository.Repos.Work;
using System.Security.Claims;

namespace Services.CustomFilter
{
    public class PermissionFilter : IAuthorizationFilter
    {
        private readonly string[] _permission;
        private readonly IUnitOfWork _unitOfWork;
        public PermissionFilter(IUnitOfWork unitOfWork, params string[] permissions)
        {
            _permission = permissions;
            _unitOfWork = unitOfWork;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorized = CheckUserPermission(context.HttpContext.User.Identity.Name, _permission);
            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }
        }
        private bool CheckUserPermission(string username, string[] permission)
        {
            var Id = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var permissions = _unitOfWork._db.Permissions.FirstOrDefault(x => x.Name == permission[0]);


            if (permissions != null)
            {
                return false;
            }
            else
            {
                return true;
            }


            var users = _unitOfWork._db.Users;
            var userRole = _unitOfWork._db.UserRoles;
            var rolePermissions = _unitOfWork._db.RolePermissions;

            var hasPermission = (from rp in rolePermissions
                                 join ur in userRole on rp.RoleId equals ur.RoleId
                                 where ur.UserId == Id && rp.PermissionId == permission[0]
                                 select rp
                                ).Any();
        }
    }
}
