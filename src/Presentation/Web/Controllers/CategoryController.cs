using Common.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Repos.Work;
using Services.BL;

namespace Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryservice _categoryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TicketingContext _db;
        public CategoryController(ICategoryservice categoryService, IUnitOfWork unitOfWork,TicketingContext db)
        {
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
            _db = db;
        }
        public IActionResult Index()
        {
            var category = _unitOfWork._db.Category.ToList();
            return View(category);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(AddCategoryViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var response = _categoryService.AddCategory(model);
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
        public IActionResult EditCategory(string CId)
        {
            if(CId == null )
            {
                return NotFound();
            }
            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.CId == CId);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult EditCategory(EditCategoryViewModel model)
        {
            var response = _categoryService.EditCategory(model);
            if(response.Status == "00")
            {
                return RedirectToAction("Edit");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult CateoryDetails(string CId)
        {
            var categoryDetails = _unitOfWork.Category.GetFirstOrDefault(u => u.CId == CId);
            var response = _categoryService.CategoryDetails(CId);
            if(response.Status == "00")
            {
                return RedirectToAction("Detail");
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet]
        public IActionResult DeleteCategory(string Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var deleteFromCategory = _unitOfWork.Category.GetFirstOrDefault(u => u.CId == Id);
            if(deleteFromCategory == null)
            {
                return NotFound();
            }
            return View(deleteFromCategory);
        }
        [HttpPost]
        public IActionResult DeleteCategory(CategoryViewModel model)
        {
            var response = _categoryService.DeleteCategory(model);
            if (response.Status == "00")
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
