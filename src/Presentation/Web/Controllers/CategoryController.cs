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
        //[PermissionFilter("Admin&Category&View_Category")]
        public IActionResult Index()
        {
            var category = _unitOfWork._db.Category.Include(x => x.Tickets).OrderBy(x => x.CategoryName).ToList();
            return View(category);
        }
        public IActionResult CategoryTicket()
        {
            var categoryTicket = _unitOfWork._db.CategoryTickets.OrderByDescending(x => x.CategoryName).ToList();
            return View();
        }
        [HttpGet]
        [PermissionFilter("Admin&Category&Create_Category")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [PermissionFilter("Admin&Category&Create_Category")]
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
        [PermissionFilter("Admin&Category&Edit_Category")]
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
        [PermissionFilter("Admin&Category&Edit_Category")]
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
        [PermissionFilter("Admin&Category&View_Category")]
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
        //[PermissionFilter("Admin&Category&Delete_Category")]
        public IActionResult DeleteCategory(string CId)
        {
            if(CId == null)
            {
                return NotFound();
            }
            var category = _unitOfWork._db.Category.FirstOrDefault(x => x.CId == CId);
            
            return View(category);
        }

        [HttpPost]

        //[PermissionFilter("Admin&Category&Delete_Category")]
        public IActionResult DeleteCategory(Category category)
        {
            var response = _categoryService.DeleteCategory(category);
            if(response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
    }
}
