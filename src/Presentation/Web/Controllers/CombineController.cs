using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entites;
using Repository.Entities;
using Repository.Repos.Work;

namespace Web.Controllers
{
    [Authorize]
    public class CombineController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CombineController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //[Permissions("Slug", "", "")]Z
        public IActionResult Index()
        {
            List<User> user = _unitOfWork._db.Users.ToList();
            List<UserRole> UserRole = _unitOfWork._db.UserRoles.ToList();
            List<Role> role = _unitOfWork._db.Roles.ToList();

            var query = from e in user
                        join d in UserRole on e.Id equals d.UserId into table1
                        from d in table1.ToList()
                        join i in role on d.RoleId equals i.Id into table2
                        from i in table2.ToList()
                        select new CommonModel
                        {
                            user = e,
                            role = i,
                            UserRole = d,
                        };
            return View(query);
        }
    }
}
