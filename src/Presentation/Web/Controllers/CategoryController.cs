using Common.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Repository.Entites;
using Repository.Repos.Work;
using Services.BL;
using Services.CustomFilter;

namespace Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryservice _categoryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        public CategoryController(ICategoryservice categoryService, IUnitOfWork unitOfWork, IToastNotification toastNotification)
        {
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            var category = _unitOfWork._db.Category.Include(x => x.Tickets).ToList();
            return View(category);
        }
        [HttpGet]
        [PermissionFilter("Category&Create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [PermissionFilter("Category&Create")]
        public IActionResult Create(AddCategoryViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var response = _categoryService.AddCategory(model);
            if (response.Status == "00")
            {
                _toastNotification.AddSuccessToastMessage("Category created sucessfully");
                return RedirectToAction("Index");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Unable to create Category");
                return View(model);
            }
        }
        [HttpGet]
        [PermissionFilter("Category&Edit")]
        public IActionResult EditCategory(string CId)
        {
            var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(x => x.CId == CId);
            if (category == null)
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
        [PermissionFilter("Category&Edit")]
        public IActionResult EditCategory(EditCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            var response = _categoryService.EditCategory(model);
            if (response.Status == "00")
            {
                _toastNotification.AddSuccessToastMessage("Category edited sucessfully");
                return RedirectToAction("Index");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Unable to edit ticket");
                return View(model);
            }
        }
        [HttpGet]
        [PermissionFilter("Category&View")]
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
            if(CId == null)
            {
                return NotFound();
            }
            var category = _unitOfWork._db.Category.FirstOrDefault(x => x.CId == CId);
            Category model = new Category();
            model = category;
            return View(model);
        }
        [HttpPost]
        public IActionResult DeleteCategory(Category category)
        {
            var response = _categoryService.DeleteCategory(category);
            if(response.Status =="00")
            {
                _toastNotification.AddWarningToastMessage("Category deleted sucessfully");
                return RedirectToAction("Index");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Unable to delete the category");
                return View(category);
            }
        }
    }
}
