//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Repository.Entities;
//using Repository.Repos.Work;

//namespace WebApplicationTicket.Controllers
//{
//    public class CategoryController : Controller
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        private readonly UserManager<User> _userManager;

//        private readonly SignInManager<User> _signInManager;

//        public CategoryController(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager)
//        {
//            _unitOfWork = unitOfWork;
//            _userManager = userManager;
//            _signInManager = signInManager;
//        }

//        public IActionResult Index()
//        {
//            IEnumerable<Category> objCategoryList = _unitOfWork._db.Category.Include("Tickets").Select(x => new Category()
//            {
//                CategoryName = x.CategoryName,
//                CId = x.CId,
//                Tickets = x.Tickets.Select(t => new Ticket()
//                {
//                    TicketDetails = t.TicketDetails,
//                    CreatedBy = t.CreatedBy,
//                    CreatedDateTime = t.CreatedDateTime,
//                    AssignedTo = t.AssignedTo,
//                    Imagefile = t.Imagefile,
//                    TicketId = t.TicketId,
//                })
//            });
//            return View(objCategoryList);
//        }
//        [HttpGet]
//        public IActionResult Create()
//        {

//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> Create(CreateCategoryViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                Category obj = new Category
//                {
//                    CategoryName = model.CategoryName,
//                    CreatedBy = model.CreatedBy,
//                    CreatedDateTime = model.CreatedDateTime = DateTime.Now,

//                };

//                {
//                    var userid = _userManager.GetUserId(HttpContext.User);
//                    User user = await _userManager.FindByIdAsync(userid);
//                    obj.User = user;
//                    var FirstName = user.FirstName;
//                    var LastName = user.LastName;
//                    obj.CreatedBy = FirstName + "" + LastName;
//                    obj.CreatedDateTime = DateTime.Now;
//                    _unitOfWork.Category.Add(obj);
//                    _unitOfWork.Save();
//                    return RedirectToAction("Index");
//                }

//            };
//            return View(model);
//        }


//        [HttpGet]
//        [Authorize(Roles = "Admin,Developer")]
//        public IActionResult Edit(int? Id)
//        {
//            if (Id == null || Id == 0)
//            {
//                return NotFound();
//            }
//            // var CategoryFromDb = _unitOfWork.Category.Find(Id);
//            var CategoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.CId == Id);
//            //var ticketFromSingle = _unitOfWork.Tickets.SingleOrDefault(u => u.TicketId == TicketId);

//            if (CategoryFromDb == null)
//            {
//                return NotFound();
//            }
//            return View(CategoryFromDb);

//        }
//        [HttpPost]
//        public async Task<IActionResult> Edit(Category obj)
//        {
//            if (ModelState.IsValid)
//            {
//                {
//                    var userid = _userManager.GetUserId(HttpContext.User);
//                    User user = await _userManager.FindByIdAsync(userid);
//                    obj.User = user;
//                    var FirstName = user.FirstName;
//                    var LastName = user.LastName;
//                    obj.ModifiedBy = FirstName + "" + LastName;
//                    obj.ModifiedDateTime = DateTime.Now;
//                    obj.ModifiedBy = user.FirstName + "" + user.LastName;
//                    _unitOfWork.Category.Update(obj);
//                    _unitOfWork.Save();
//                    return RedirectToAction("Index");
//                }
//            }
//            return View(obj);
//        }
//        [Authorize(Roles = "Admin,Developer,User")]
//        public IActionResult Details(int? Id)
//        {

//            if (Id == null)
//            {
//                return NotFound();
//            }

//            var user = _unitOfWork.Category.
//                GetFirstOrDefault(u => u.CId == Id);
//            if (user == null)
//            {
//                return NotFound();
//            }

//            return View(user);
//        }
//        [Authorize(Roles = "Admin")]
//        [HttpGet]
//        public IActionResult Delete(int? Id)
//        {
//            if (Id == null || Id == 0)
//            {
//                return NotFound();
//            }
//            // var categoryFromDb = _unitOfWork.Category.Find(Id);
//            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.CId == Id);

//            if (categoryFromDb == null)
//            {
//                return NotFound();
//            }
//            return View(categoryFromDb);
//        }


//        //POST
//        [HttpPost]
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Admin")]
//        public IActionResult DeletePOST(int? Id)
//        {
//            //var obj = _unitOfWork.Category.Find(Id);
//            var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.CId == Id);
//            if (obj == null)
//            {
//                return NotFound();
//            }
//            {
//                _unitOfWork.Category.Remove(obj);
//                _unitOfWork.Save();
//                TempData["sucess"] = "Category deleted Sucessfully";
//                return RedirectToAction("Index");
//            }

//        }
//    }
//}
