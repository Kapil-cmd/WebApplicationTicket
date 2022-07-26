using Common.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Repository.Repos.Work;
using Services.BL;

namespace Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryservice _categoryService;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(ICategoryservice categoryService, IUnitOfWork unitOfWork)
        {
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
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
           if(!ModelState.IsValid)
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
        public IActionResult EditCategory(int Id)
        {
            var categoryDetails = _unitOfWork._db.Category.Find(Id);
            return View(categoryDetails);
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
        public IActionResult CateoryDetails(int CId)
        {
            var categoryDetails = _unitOfWork._db.Category.Find(CId);
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
        public IActionResult DeleteCategory(int Id)
        {
          var delete = _unitOfWork.Category.GetById(Id);
            return View(delete);
        }
        [HttpPost]
        public IActionResult DeleteUser(CategoryViewModel model)
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
