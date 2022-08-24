using Common.ViewModels.Role;
using Common.ViewModels.RolePermission;
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
            IEnumerable<Role> roles = _unitOfWork._db.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateRole(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            var response = _roleService.CreateRole(model);
            if(response.Status == "00")
            {
                return View("Index");
            }
            else
            {
                return View("CreateRole");
            }
        }
        [HttpGet]
        public IActionResult ManageRole(string Id)
        {
            var role = _unitOfWork._db.Roles.FirstOrDefault(x => x.Id == Id);
            if (role == null)
            {
                return NotFound();
            }
            EditRole model = new EditRole();
            model.Name = role.Name;
            var permission = _unitOfWork.Permission.GetAll();
            if(permission != null)
            {
                if(permission.Count() > 0)
                {
                    model.ListPermission = permission.Select(x => new ListPermission()
                    {
                        Id = x.PermissionId,
                        Name = x.Name,
                    }).ToList();
                }
            }

            return View(model);

        }
        [HttpPost]
        public IActionResult ManageRole(EditRole model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var response = _roleService.ManageRole(model);
            if(response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult RoleDetails (string Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var role = _unitOfWork._db.Roles.Find(Id);
            if(role == null)
            {
                return NotFound();
            }
            else
            {
                return View(role);
            };
        }
        public IActionResult Delete(string Id)
        {
            var deleteRole = _unitOfWork._db.Roles.Find(Id);
            return View(deleteRole);
        }
        [HttpPost]
        public IActionResult Delete(RoleViewModel model)
        {
            var response = _roleService.DeleteRole(model);
            if(response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult AssignPermissionToRole(string Id)
        {
            if(Id == null)
            {
                return NotFound();
            }    
            var role = _unitOfWork._db.Roles.Find(Id);
            if(role == null)
            {
                return NotFound();
            }
             RolePermissionViewModel model= new RolePermissionViewModel();
            var permission = _unitOfWork._db.Permissions.ToList();
            if(permission != null)
            {
                model.Permissions = permission.Select(x => new PermissionList()
                {
                    Id = x.PermissionId,
                    PermissionName = x.Name
                }).ToList();
            }
            return View(model);
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
