using Common.ViewModels.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Entites;
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
        [PermissionFilter("Admin&Permission&Create_Permission")]
        [HttpGet]
        public IActionResult AddPermission()
        {
            return View();
        }
        [HttpPost]
        [PermissionFilter("Admin&Permission&Create_Permission")]
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
        [PermissionFilter("Admin&Permission&Edit_Permission")]
        public IActionResult Edit(string? id)
        {
            var permission = _unitOfWork.Permission.GetFirstOrDefault(x => x.PermissionId == id);
            EditPermission model = new EditPermission();
            model.PermissionId = permission.PermissionId;
            model.Name = permission.Name;
            return View(model);
        }
        [HttpPost]
        [PermissionFilter("Admin&Permission&Edit_Permission")]
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
        [PermissionFilter("Admin&Permission&Delete_Permission")]
        public IActionResult DeletePermission(string? PermissionId)
        {
            var permission = _unitOfWork._db.Permissions.FirstOrDefault(x => x.PermissionId == PermissionId);
            Permission model = new Permission();
            permission = model;
            return View(permission);
        }
        [HttpPost]
        [PermissionFilter("Admin&Permission&Delete_Permission")]
        public IActionResult DeletePermission(Permission model)
        {
            var response = _permissionService.DeletePermission(model);
            if (response.Status == "00")
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
