using Common.ViewModels.Role;
using Common.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Repository.Entites;
using Repository.Repos.Work;
using Services.BL;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUserService userService,IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(UserRegister model)
        {
            if(!ModelState.IsValid)
            {
                // Message return 
                return View(model);
            }
            var response = _userService.Register(model);
            if(response.Status == "00")
            {
                // redirect to login page
                return RedirectToAction("Login");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult EditUser(string? Id)
        {

            var userDetail = _unitOfWork._db.Users.Find(Id);
            List<Role> role = _unitOfWork._db.Roles.ToList();

            var UserRoles = new UserRole
            {
                aUser = userDetail,
                Roles = role
            };
            return View(userDetail); 
        }
        [HttpPost]
        public IActionResult EditUser(EditUserViewModel model)
        {
            var response = _userService.EditUser(model);
            if(response.Status == "00")
            {
                //redirect to user profile page
                return RedirectToAction("Profile");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult UserDetails(string Id)
        {
            var response = _userService.UserDetails(Id);
            if(response.Status =="00")
            {
                return RedirectToAction("Details", "User");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public IActionResult DeleteUser(string? Id)
        {
            var user = _unitOfWork.UserRepository.GetById(Id);
            return View(user);
        }
        [HttpPost]
        public IActionResult DeleteUser(UserViewModel model)
        {
            var response = _userService.DeleteUser(model);
            if(response.Status == "00")
            {
                return RedirectToAction("DeleteUser");
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLogin model)
        {
            var resopnse = _userService.LoginAsync(model).Result;
            if (resopnse.Status == "00")
            {
                //User is authorized
                //Route user to home page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // User is not authorized
                // Redirect to user login page
                return View(model);
            }
        }
        [HttpPost]
        public IActionResult AssignRole(string userId, string roleId)
        {
            var response = _userService.AssignUserToRole(userId, roleId);
            if(response.Status == "00")
            {
                return View("Edit", "Role");
            }
            else
            {
                return View("Index", "Role");
            }
        }
        public IActionResult RemoveRole(string userId,String roleId)
        {
            var response = _userService.RemoveUserFromRole(userId, roleId);
            if (response.Status == "00")
            {
                return RedirectToAction("Index", "Role");
            }
            else
            {
                return RedirectToAction("Edit", "Role");
            }
        }
    }
}
