using Common.ViewModels.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Entites;
using Repository.Repos.Work;
using Services.BL;
using System.Net;
using System.Net.Mail;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TicketingContext _db;
        public UserController(IUserService userService, IUnitOfWork unitOfWork,TicketingContext db)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
            _db = db;
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
            bool Status = false;
            string message = "";

            #region Email Verified
            var isExist = IsEmailExist(model.Email);
            if (isExist)
            {
                ModelState.AddModelError("EmailExist", "Email already exist");
                return View(model);
            }
            #endregion
            
            if (!ModelState.IsValid)
            {
                // Message return 
                return View(model);
            }
            var response = _userService.Register(model);
            if (response.Status == "00")
            {
                SendVerificationLinkEmail(model.Email, model.ActivationCode.ToString());
                message = "Registration sucessfully done.Account activation link" + "has been send to your email id:" + model.Email;
                Status = true;
                
                // redirect to login page
                return RedirectToAction("RegisterUser");
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
            var user = _unitOfWork._db.Users.FirstOrDefault(x => x.Id == Id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel();
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;
            model.Address = user.Address;
            model.PhoneNumber = user.PhoneNumber;
            var role = _unitOfWork._db.Roles.ToList();
            if(role != null)
            {
                if(role.Count() > 0) 
                { 

                    model.Roles = role.Select( x => new Common.ViewModels.Users.ListRole()
                    {
                    Id = x.Id,
                    Name = x.Name,
                    }).ToList();
                }
            }
            return View(model);

        }
        [HttpPost]
        public IActionResult EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var response = _userService.EditUser(model);

            if (response.Status == "00")
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
            if (Id == null)
            {
                return NotFound();
            }
            var response = _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == Id);
            if (response == null)
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
            if (response.Status == "00")
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
                if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
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
        [HttpGet]
        public IActionResult RemoveRole(string Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var user = _unitOfWork._db.Users.Find(Id);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [NonAction]
        public bool IsEmailExist(string Email)
        {
            
                var v = _unitOfWork._db.Users.Where(x => x.Email == Email).FirstOrDefault();
                return v != null;
            
        }
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;

            var link = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + verifyUrl;

            var fromEmail = new MailAddress("dashcharging7@gmail.com", "Kapil");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "qygqezeyxisnoavl"; 
            string subject = "Your account is successfully created!";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            var Message = new MailMessage(fromEmail, toEmail);
            Message.Subject = "Registration Completed";
            Message.Body = "<br/> Your registration completed succesfully." +
                   "<br/> please click on the below link for account verification" +
                   "<br/><br/><a href=" + link + ">" + link + "</a>";
            Message.IsBodyHtml = true;
             smtp.Send(Message);
        }
        [HttpGet]
        public IActionResult VerifyAccount(string id)
        {
            bool Status = false;

            var IsVerify = _unitOfWork._db.Users.Where(x => x.ActivationCode == new Guid(id)).FirstOrDefault();
            if(IsVerify != null)
            {
                IsVerify.IsEmailVerified = true;
                _unitOfWork._db.SaveChanges();
                ViewBag.Message = "Email Verification completed";
                Status = true;
            }
            else
            {
                ViewBag.Message = "Invalid Request";
                ViewBag.Status = false;
            }
            
            return View();
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChangePassword(ChangePassword model)
        {
            return View(model);
        }
        [HttpPost,ActionName("ChangePassword")]
        public IActionResult ChangePasswordPOST(ChangePassword model)
        {
            var response = _userService.ChangePassword(model);
            if(response.Status == "00")
            {
                return RedirectToAction("Homepage", "Home");
            }
            return View(model);

        }
    }
}
