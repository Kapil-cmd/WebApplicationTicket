using Common.ViewModels.Role;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Entites;
using Repository.Repos.Work;
using Services.BL;

namespace Web.Controllers
{
    public class RoleController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly TicketingContext _db;
        public readonly IRoleService _roleService;

        public RoleController(IUnitOfWork unitOfWork, TicketingContext db, IRoleService roleService)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateRole(RoleViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var response = _roleService.CreateRole(model);
            if(response.Status == "00")
            {
                return View("Create");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult EditRole(string? id)
        {
            var editRole = _unitOfWork._db.Roles.Find(id);
            IEnumerable<Permission> permissionList = _unitOfWork.Permission.GetAll();

            var permission = new RolePermission
            {
                aRole = editRole,
                Permissions = permissionList
            };
            return View(permission);

        }
        [HttpPost]
        public IActionResult EditRole(EditRole model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var response = _roleService.EditRole(model);
            if(response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public IActionResult AssignPermissionToRole(string roleId,string permissionId)
        {
            var response = _roleService.AssignPermissionToRole(roleId, permissionId);
            if(response.Status =="00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index","Role");
            }
        }
        [HttpPost]
        public IActionResult RemovePermissionFromRole(string roleId,string permissionId)
        {
            var response = _roleService.RemovePermissionFromRole(roleId, permissionId);
            if(response.Status =="00")
            {
                return RedirectToAction("Index");
            }    
            else
            {
                return RedirectToAction("Index", "Role");
            }
        }
    }
}
