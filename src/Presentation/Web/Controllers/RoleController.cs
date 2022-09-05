﻿using Common.ViewModels.Role;
using Common.ViewModels.RolePermission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                _toastNotification.AddSuccessToastMessage("Role created sucessfully");
                return View("Index");
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
                        Name = x.Slug,
                    }).OrderBy(x => x.ControllerName).ToList();
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
        [HttpGet]
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
