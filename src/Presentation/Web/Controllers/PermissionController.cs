using Common.ViewModels.Permission;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Repos.Work;
using Services.BL;
using Services.CustomFilter;

namespace Web.Controllers
{
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
        public IActionResult Index()
        {
            var permission = _unitOfWork._db.Permissions.ToList();
            return View(permission);
        }
        [HttpGet]
        public IActionResult AddPermission()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPermission(AddPermission model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var response = _permissionService.AddPermission(model);
            if (response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult Edit(string? id)
        {
            var permission = _unitOfWork.Permission.GetFirstOrDefault(x => x.PermissionId == id);
            EditPermission model = new EditPermission();
            model.PermissionId = permission.PermissionId;
            model.Name = permission.Name;
            model.ControllerName = permission.ControllerName;
            model.ActionName = permission.ActionName;
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(EditPermission model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var response = _permissionService.EditPermission(model);
            if(response.Status =="00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult Delete(string? id)
        {
            var permission = _unitOfWork._db.Permissions.FirstOrDefault(x => x.PermissionId == id);
            return View(permission);
        }
        [HttpPost]
        public IActionResult Delete(PermissionViewModel model)
        {
            var response = _permissionService.DeletePermission(model);
            if(response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
    }
}
