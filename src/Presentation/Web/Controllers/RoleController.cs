using Common.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Repository;
using Repository.Entites;
using Repository.Repos.Work;
using Services.BL;
using Services.CustomFilter;

namespace Web.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly TicketingContext _db;
        public readonly IRoleService _roleService;
        private readonly IToastNotification _toastNotification;

        public RoleController(IUnitOfWork unitOfWork, TicketingContext db, IRoleService roleService,IToastNotification toastNotification)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _roleService = roleService;
            _toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            var roles = _unitOfWork._db.Roles.ToList();
            return View(roles);
        }
        [PermissionFilter("Admin&Role&User_Role")]
        public IActionResult UserRole()
        {
            var userRole = _unitOfWork._db.UserRoles.ToList();
            return View(userRole);
        }
        [HttpGet]
        //[PermissionFilter("Admin&Role&Create_Role")]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        //[PermissionFilter("Admin&Role&Create_Role")]
        public IActionResult CreateRole(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            var response = _roleService.CreateRole(model);
            if(response.Status == "00")
            {
                _toastNotification.AddSuccessToastMessage("Role created sucessfully");
                return RedirectToAction("Index");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Unable to create role");
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
                        MenuName = x.MenuName,
                    }).OrderBy(x => x.MenuName).ToList();
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
                _toastNotification.AddSuccessToastMessage("Role edited sucessfully");
                return RedirectToAction("Index");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Unable to edit role");
                return View(model);
            }
        }
        [HttpGet]
        [PermissionFilter("Admin&Role&Details_Role")]
        public IActionResult RoleDetails (string Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var role = _unitOfWork._db.Roles.Include("Permissions").Include("Permissions.aPermission").FirstOrDefault(x => x.Id == Id);
            if (role == null)
            {
                return NotFound();
            }
            else
            {
                return View(role);
            };
        }
        [HttpGet]
        //[PermissionFilter("Admin&Role&Delete_Role")]
        public IActionResult Delete(string Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var deleteRole = _unitOfWork._db.Roles.Find(Id);
            return View(deleteRole);
        }
        [HttpPost]
        //[PermissionFilter("Admin&Role&Delete_Role")]
        public IActionResult Delete(Role model)
        {
            var response = _roleService.DeleteRole(model);
            if(response.Status == "00")
            {
                _toastNotification.AddSuccessToastMessage("Role deleted sucessfully");
                return RedirectToAction("Index");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Unable to delete role");
                return View(model);
            }
        }
    }
}
