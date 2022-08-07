using Common.ViewModels.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entites;
using Repository.Repos.Work;
using Services.BL;
using System.Collections.Generic;
using System.Linq;

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
        public IActionResult Index()
        {
            IEnumerable<User> userList = _unitOfWork.UserRepository.GetAll();
            return View(userList);
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
        [Authorize]
        public IActionResult EditUser(string Id)
        {
            var user = _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == Id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Email = user.Email,
                Age = user.Age,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
            };
            return View(model);

        }
        [HttpPost]
        public IActionResult EditUser(EditUserViewModel model)
        {
            var response = _userService.EditUser(model);
            if(response.Status == "00")
            {
                //redirect to user profile page
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult UserDetails(string Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var response = _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == Id);
            if( response == null)
            {
                return NotFound();
            }
            else
            {
                return View(response);
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

        public IActionResult Login(string ReturnUrl = null)
        {
            UserLogin model = new UserLogin()
            {
                ReturnUrl = ReturnUrl
            };
            return View(model);
        }

        public IActionResult Logout()
        {
            _unitOfWork._httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin model)
        {
            var resopnse = await _userService.LoginAsync(model);
            if (resopnse.Status == "00")
            {
                //User is authorized
                //Route user to home page
                if(!string.IsNullOrWhiteSpace(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction("HomePage", "Home");
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
