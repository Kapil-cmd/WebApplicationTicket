using Common.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Repository.Repos.Work;
using Services.BL;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
        }
        [HttpGet]
        public IActionResult EditUser(string? Id)
        {
        }
        [HttpPost]
        public IActionResult EditUser(User user)
        {
        }
        public IActionResult Details(string? Id)
        {
        }
        [HttpGet]
        public IActionResult DeleteUser(string? Id)
        {
        }
        [HttpPost]
        public IActionResult DeleteUser(User user)
        {
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLogin model)
        {
            var resopnse = _userService.Login(model);
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

    }
}
