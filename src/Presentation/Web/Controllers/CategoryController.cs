using Common.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Entites;
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
            var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(x => x.CId == CId);
            if(category == null)
            {
                return NotFound();
            }
            EditCategoryViewModel model = new EditCategoryViewModel
            {
                CId = category.CId,
                CategoryName = category.CategoryName
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult EditCategory(EditCategoryViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("Index");
            }
            var response = _categoryService.EditCategory(model);
            if(response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult CategoryDetails(string CId)
        {
            if (CId == null)
            {
                return NotFound();
            }
            var categoryDetails = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.CId == CId);
            if (CategoryDetails == null)
            {
                return NotFound();
            }
            else
            {
                return View(categoryDetails);
            }
        }
        [HttpGet]
        public IActionResult DeleteCategory(string CId)
        {
            if (CId == null)
            {
                return NotFound();
            }
            var deleteFromCategory = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.CId == CId);
            if (deleteFromCategory == null)
            {
                return NotFound();
            }
            return View(deleteFromCategory);

        }
        [HttpPost, ActionName("DeleteCategory")]
        public IActionResult DeleteCategoryPost(Category model)
        {
            var response = _categoryService.DeleteCategory(model);
            if(response.Status == "00")
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
