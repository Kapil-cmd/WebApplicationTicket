using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Repos.Work;
using Services.BL;
using Services.CustomFilter;

namespace Web.Controllers
{
    [Authorize]
    public class PermissionController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly TicketingContext _db;
        public readonly IUserService _userService;
        public readonly IPermissionService _permissionService;

        public PermissionController(IUnitOfWork unitOfWork, TicketingContext db,IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _permissionService = permissionService;
        }
        [PermissionFilter("Admin&Permission&View_Permission")]
        public IActionResult Index()
        {
            var permission = _unitOfWork._db.Permissions.ToList();
            return View(permission);
        }
       
    }
}
