  using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Repos.Work;
using Services.BL;

namespace Web.Controllers
{
    public class PermissionController:Controller
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
            return View();
        }
        [HttpGet]
        public IActionResult AddPermission()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPermission(PermissionViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var response = _permissionService.AddPermission(model);
            if (response.Status == "00")
            {
                return RedirectToAction("AddPermission");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var permission = _unitOfWork.Permission.GetById(id);
            return View(permission);
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
                return RedirectToAction("Edit");
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
                return RedirectToAction("Delete");
            }
            else
            {
                return View(model);
            }
        }
    }
}
