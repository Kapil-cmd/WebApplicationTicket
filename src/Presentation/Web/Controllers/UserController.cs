using Common.ViewModels.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Repository;
using Repository.Entites;
using Repository.Repos.Work;
using Services.BL;
using Services.CustomFilter;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using X.PagedList;

namespace Web.Controllers
{
    
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public readonly IWebHostEnvironment _webHostEnvironment;
        private readonly TicketingContext _db;
        private readonly IToastNotification _toastNotification;
        public UserController(IUserService userService, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, TicketingContext db, IToastNotification toastNotification)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _db = db;
            _toastNotification = toastNotification;
        }
        [PermissionFilter("Admin&User&View_User")]
        public IActionResult Index(string sortOrder, string searchString,string currentFilter,int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortparm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AgeSortParm = String.IsNullOrEmpty(sortOrder)? "age_desc":"";

            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.currentFilter = searchString;

            var user = from users in _unitOfWork._db.Users
                       select users;
            if (!String.IsNullOrEmpty(searchString))
            {
                user = user.Where(x => x.UserName.Contains(searchString)
                || x.Email.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    user = user.OrderByDescending(x => x.UserName);
                    break;
                case "age_desc":
                    user = user.OrderByDescending(x => x.Age);
                    break;
                default:
                    user = user.OrderByDescending(x => x.UserName.StartsWith("A"));
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(user.ToPagedList(pageNumber, pageSize));

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
                _toastNotification.AddWarningToastMessage("Email already exists");
                return View(model);
            }
            #endregion

            if (!ModelState.IsValid)
            {
                _toastNotification.AddAlertToastMessage("Fill all the form carefully");
                return View(model);
            }
            var response = _userService.Register(model);
            if (response.Status == "00")
            {
                SendVerificationLinkEmail(model.Email, model.ActivationCode.ToString());
                message = "Registration sucessfully done.Account activation link" + "has been send to your email id:" + model.Email;
                _toastNotification.AddAlertToastMessage("Please check your email to verify your account");
                Status = true;

                // redirect to login page
                _toastNotification.AddSuccessToastMessage("User registered sucessfully");
                return RedirectToAction("RegisterUser");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        [Authorize]
        [PermissionFilter("Admin&User&Edit_User")]
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
            model.UserName = user.UserName;
            var role = _unitOfWork._db.Roles.ToList();
            if (role != null)
            {
                if (role.Count() > 0)
                {

                    model.Roles = role.Select(x => new Common.ViewModels.Users.ListRole()
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList();
                }
            }
            return View(model);

        }
        [HttpPost]
        [PermissionFilter("Admin&User&Edit_User")]
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
                _toastNotification.AddSuccessToastMessage("User edited sucessfully");
                return RedirectToAction("Index");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Unable to edit the User");
                return View(model);
            }
        }
        [HttpGet]
        [PermissionFilter("Admin&User&View_User")]
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
        [PermissionFilter("Admin&User&delete_User")]
        public IActionResult DeleteUser(string? Id)
        {
            var user = _unitOfWork._db.Users.FirstOrDefault(x => x.Id == Id);
            return View(user);
        }
        [HttpPost]
        [PermissionFilter("Admin&User&Delete_User")]
        public IActionResult DeleteUser(User model)
        {
            var response = _userService.DeleteUser(model);
            if (response.Status == "00")
            {
                _toastNotification.AddSuccessToastMessage("User deleted sucessfully");
                return RedirectToAction("DeleteUser");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Can't delete the User");
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
                _toastNotification.AddSuccessToastMessage("Login sucess");
                return RedirectToAction("HomePage", "Home");
            }
            else
            {
                // User is not authorized
                // Redirect to user login page
                _toastNotification.AddErrorToastMessage("Unable to login");
                return View(model);
            }
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

            var fromEmail = new MailAddress("creation.soft123@gmail.com", "Please Verify your Account");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "zsudgukujageoqqc";

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
            if (IsVerify != null)
            {
                IsVerify.IsEmailVerified = true;
                _unitOfWork._db.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Email sucessfully verified");
                ViewBag.Message = "Email Verification completed";
                Status = true;
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Invalid request");
                ViewBag.Message = "Invalid Request";
                ViewBag.Status = false;
            }

            return View();
        }
        [HttpGet]
        public IActionResult ChangePassword(ChangePassword model)
        {
            return View(model);
        }
        [HttpPost, ActionName("ChangePassword")]
        public IActionResult ChangePasswordPOST(ChangePassword model)
        {
            var response = _userService.ChangePassword(model);
            if (response.Status == "00")
            {
                _toastNotification.AddSuccessToastMessage("Password changed sucessfully");
                return RedirectToAction("Homepage", "Home");
            }
            _toastNotification.AddErrorToastMessage("Unable to change the password");
            return View(model);

        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPassword(ForgetPassword password)
        {
            var IsExists = IsEmailExist(password.Email);
            if (!IsExists)
            {
                _toastNotification.AddAlertToastMessage("This email doesn't exists");
                ModelState.AddModelError("EmailNotExists", "This email doesn't exists");
                return View();
            }
            var user = _unitOfWork._db.Users.FirstOrDefault(x => x.Email == password.Email);

            string OTP = GeneratePassword();

            user.ActivationCode = Guid.NewGuid();
            user.OTP = OTP;
            _db.SaveChanges();

            ForgetPasswordEmailToUser(user.Email, user.ActivationCode.ToString(), user.OTP);
            return View();
        }

        [NonAction]
        public void ForgetPasswordEmailToUser(string emailID, string activationCode, string OTP)
        {
            var verifyUrl = "/User/ResetPassword/" + activationCode;

            var link = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + verifyUrl;

            var fromEmail = new MailAddress("creation.soft123@gmail.com", "Change Password");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "zsudgukujageoqqc";

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
            Message.Subject = "Password Reset";
            Message.Body = "<br/> You can sucessfully change your password using OTP." +
                   "<br/> please click on the below link for password change" +
                   "<br/><br/><a href=" + link + ">" + link + "</a>" + OTP;
            Message.IsBodyHtml = true;
            smtp.Send(Message);
        }
        private string GeneratePassword()
        {
            string OTPLength = "4";
            string OTP = string.Empty;

            string Chars = string.Empty;
            Chars = "1,2,3,4,5,6,7,8,9,0";

            char[] splitChar = { ',' };
            string[] arr = Chars.Split(splitChar);
            string NewOTP = "";
            string temp = "";
            Random random = new Random();
            for (int i = 0; i < Convert.ToInt32(OTPLength); i++)
            {
                temp = arr[random.Next(0, arr.Length)];
                NewOTP += temp;
                OTP = NewOTP;
            }
            return OTP;
        }
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPassword password)
        {
            var response = _userService.ResetPassword(password);
            if(response.Status == "00")
            {
                _toastNotification.AddSuccessToastMessage("Password reset sucessfully");
                return RedirectToAction("Login", "User");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Unable to reset the password");
                return View(password);
            }
        }
        [HttpGet]
        public IActionResult UserProfile(string? Id)
        {
            var userId = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork._db.Users.FirstOrDefault(x => x.Id == userId);
            UserProfile model = new UserProfile();
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.PhoneNumber = user.PhoneNumber;
            model.Address = user.Address;
            model.ImageName = user.ProfilePicture;
            model.DateOfBirth = user.DateOfBirth;
            model.Email = user.Email;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UserProfile(UserProfile model)
        {
            if (model.ProfilePic != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(model.ProfilePic.FileName);
                string extension = Path.GetExtension(model.ProfilePic.FileName);
                model.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                string path = Path.Combine(wwwRootPath + "/Profile", fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await model.ProfilePic.CopyToAsync(filestream);
                }
            }
            var response = _userService.UserProfile(model);
            if (response.Status == "00")
            {
                _toastNotification.AddSuccessToastMessage("User profile");
                return RedirectToAction("HomePage", "Home");
            }
            else
            {
                return View(model);
            }
        }

    }
}

       
    

