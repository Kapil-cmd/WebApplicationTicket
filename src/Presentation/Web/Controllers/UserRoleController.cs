using Microsoft.AspNetCore.Mvc;
using Repository.Repos.Work;

namespace Web.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var userRole = _unitOfWork._db.UserRoles.ToList();
            return View(userRole);
        }
    }
}
