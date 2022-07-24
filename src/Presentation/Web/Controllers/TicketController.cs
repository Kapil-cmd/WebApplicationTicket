//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Repository.Entities;
//using Repository.Repos.Work;

//namespace demo.Controllers
//{

//    public class TicketController : Controller
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly IWebHostEnvironment _webHostEnvironment;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly IUnitOfWork _unitOfWork;

//        public TicketController(UserManager<User> userManager, IWebHostEnvironment webHostEnvironment, RoleManager<IdentityRole> roleManager,
//            IUnitOfWork unitOfWork)
//        {
//            _userManager = userManager;
//            _webHostEnvironment = webHostEnvironment;
//            _roleManager = roleManager;
//            _unitOfWork = unitOfWork;
//        }

//        [Authorize(Roles = "User,Admin")]
//        public async Task<IActionResult> Index()
//        {
//            var objFromDb = _unitOfWork._db.Tickets.Include(b => b.Category);
//            return View(await objFromDb.ToListAsync());

//        }


//        //GET
//        [HttpGet]
//        public async Task<IActionResult> Create()
//        {
//            List<Category> category = await _unitOfWork._db.Category.ToListAsync();
//            if (category == null)
//            {
//                return NotFound();
//            }
//            CreateTicketViewModel model = new CreateTicketViewModel
//            {
//                Categories = category,
//            };
//            return View(model);

//        }


//        //POST
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(CreateTicketViewModel model)
//        {

//            if (ModelState.IsValid)
//            {
//                Ticket ticket = new Ticket
//                {
//                    TicketId = model.TicketId,
//                    TicketDetails = model.TicketDetails,
//                    CreatedBy = model.CreatedBy,
//                    CreatedDateTime = model.CreatedDateTime,
//                    Status = (WebApplicationTicket.Models.Status)model.Status,
//                    CategoryName = model.CategoryName,
//                    Imagefile = model.Imagefile,
//                    ImageName = model.ImageName,
//                    Category = model.Category,
//                    Categories = model.Categories,
//                    User = model.User,
//                };

//                {
//                    var userid = _userManager.GetUserId(HttpContext.User);
//                    User user = await _userManager.FindByIdAsync(userid);
//                    var FirstName = user.firstname;
//                    var LastName = user.lastname;
//                    ticket.User = user;
//                    ticket.CreatedBy = FirstName + "" + LastName;
//                    ticket.CreatedDateTime = DateTime.Now;
//                    ticket.Status = WebApplicationTicket.Models.Status.Pending;
//                    ticket.CreatedBy = user.firstname + "" + user.lastname;
//                    if (ticket.Imagefile != null)
//                    {
//                        string wwwRootPath = _webHostEnvironment.WebRootPath;
//                        string fileName = Path.GetFileNameWithoutExtension(model.Imagefile.FileName);
//                        string extension = Path.GetExtension(model.Imagefile.FileName);
//                        ticket.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
//                        string path = Path.Combine(wwwRootPath + "/Image", fileName);
//                        using (var fileStream = new FileStream(path, FileMode.Create))
//                        {
//                            await model.Imagefile.CopyToAsync(fileStream);
//                        }
//                    }
//                }
//                _unitOfWork._db.Tickets.Add(ticket);
//                _unitOfWork._db.SaveChanges();

//                ViewData["Id"] = new SelectList(_unitOfWork._db.Category, "CId", "CId", ticket.TicketId);
//                ViewData["CategoryName"] = new SelectList(_unitOfWork._db.Category, "CategoryName", "CategoryName", ticket.TicketId);

//                return RedirectToAction("Index");

//            };
//            return View(model);
//        }


//        //GET
//        [HttpGet]
//        [Authorize(Roles = "Admin,Developer")]
//        public async Task<IActionResult> Edit(int? TicketId)
//        {
//            if (TicketId == 0 || TicketId == null)
//            {
//                return NotFound();
//            }
//            var ticketFromDb = _unitOfWork._db.Tickets.Find(TicketId);
//            List<ApplicationUser> objUserList = await _unitOfWork._db.Users.ToListAsync();

//            if (ticketFromDb == null || objUserList == null)
//            {
//                return NotFound();
//            }
//            EditTicketViewModel model = new EditTicketViewModel
//            {
//                Tickets = ticketFromDb,
//                Users = objUserList
//            };
//            return View(model);

//        }


//        //POST
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Admin,Developer")]
//        public async Task<IActionResult> Edit(EditTicketViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                Ticket ticket = new Ticket
//                {
//                    TicketId = model.TicketId,
//                    TicketDetails = model.TicketDetails,
//                    ModifiedBy = model.ModifiedBy,
//                    ModifiedDateTime = model.ModifiedDateTime,
//                    AssignedTo = model.AssignedTo,
//                    ImageName = model.ImageName,
//                    Status = model.Status,
//                    User = model.User,
//                    Users = model.Users,
//                    Category = model.Category,
//                    Categories = model.Categories,
//                };

//                var userid = _userManager.GetUserId(HttpContext.User);
//                User user = await _userManager.FindByIdAsync(userid);
//                var FirstName = user.FirstName;
//                var LastName = user.LastName;
//                ticket.ModifiedBy = FirstName + "" + LastName;
//                ticket.ModifiedDateTime = DateTime.Now;
//                ticket.Status = Status.InProcess;
//                _unitOfWork._db.Tickets.Update(ticket);
//                _unitOfWork._db.SaveChanges();
//                ViewData["Id"] = new SelectList(_unitOfWork._db.Category, "CId", "CId", ticket.TicketId);
//                ViewData["CategoryName"] = new SelectList(_unitOfWork._db.Category, "CategoryName", "CategoryName", ticket.TicketId);
//                return RedirectToAction("Index");
//            }
//            return View(model);
//        }


//        [Authorize(Roles = "Admin,Developer,User")]
//        public async Task<IActionResult> Details(int? TicketId)
//        {

//            if (TicketId == null)
//            {
//                return NotFound();
//            }

//            var user = await _unitOfWork._db.Tickets.
//                FirstOrDefaultAsync(u => u.TicketId == TicketId);
//            if (user == null)
//            {
//                return NotFound();
//            }

//            return View(user);
//        }


//        //GET
//        [Authorize(Roles = "Admin")]
//        [HttpGet]
//        public IActionResult Delete(int? TicketId)
//        {
//            if (TicketId == null || TicketId == 0)
//            {
//                return NotFound();
//            }
//            var ticketFromDb = _unitOfWork._db.Tickets.Find(TicketId);
//            //var ticketFromFirst=_unitOfWork._db.Tickets.FirstOrDefault(u => u.TicketId==TicketId);
//            //var ticketFromSingle = _unitOfWork._db.Tickets.SingleOrDefault(u => u.TicketId == TicketId);

//            if (ticketFromDb == null)
//            {
//                return NotFound();
//            }
//            return View(ticketFromDb);
//        }


//        //POST
//        [HttpPost]
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Admin")]
//        public IActionResult DeletePOST(int? TicketId)
//        {
//            var obj = _unitOfWork._db.Tickets.Find(TicketId);
//            if (obj == null)
//            {
//                return NotFound();
//            }
//            _unitOfWork._db.Tickets.Remove(obj);
//            _unitOfWork._db.SaveChanges();
//            TempData["sucess"] = "Ticket deleted Sucessfully";
//            return RedirectToAction("Index");
//        }
//        [HttpGet]
//        public IActionResult AssignTo(Ticket ticket)
//        {
//            if (ticket == null)
//            {
//                return NotFound();
//            }
//            var tickets = _unitOfWork._db.Tickets.Find(ticket);
//            if (ticket == null)
//            {
//                return NotFound();
//            }

//            return View("Ticket", "Edit");
//        }

//        [HttpPost]
//        public IActionResult AssignTo(Developer developer)
//        {
//            Ticket ticket = new Ticket
//            {
//                TicketDetails = developer.Ticket.TicketDetails,
//                CreatedBy = developer.Ticket.CreatedBy,
//                CreatedDateTime = developer.Ticket.CreatedDateTime,
//                ModifiedBy = developer.Ticket.ModifiedBy,
//                ModifiedDateTime = developer.Ticket.ModifiedDateTime,
//                AssignedTo = developer.Ticket.AssignedTo,
//                Status = developer.Ticket.Status,
//                ImageName = developer.Ticket.ImageName
//            };
//            if (User == null)
//            {
//                return NotFound();
//            }
//            _unitOfWork._db.Add(developer);
//            _unitOfWork._db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//    }
//}
