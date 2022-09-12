using Common.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Repository;
using Repository.Entities;
using Services.BL;

namespace Web.Controllers
{
    public class CategoryTempController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly TicketingContext _db;
        private readonly ICategoryservice _categoryService;

        public CategoryTempController(IWebHostEnvironment environment,TicketingContext db,ICategoryservice categoryservice)
        {
            _webHostEnvironment = environment;
            _db = db;
            _categoryService = categoryservice;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var company = _db.ExcelUpdate.ToList();
            return View(company);
        }
        [HttpGet]
        public IActionResult GetExcel(List<CategoryTemp> excel)
        {
            excel = excel == null ? new List<CategoryTemp>() : excel;
            return View(excel);
        }
        [HttpPost]
        public IActionResult GetExcel(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if(file?.Length > 0)
                {
                    //convert to a stream
                    var stream = file.OpenReadStream();

                    List<CategoryTemp> companies = new List<CategoryTemp>();
                    try
                    {
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            

                            for (var row = 2; row<= rowCount; row++)
                            {
                                try
                                {
                                    var CategoryName  = worksheet.Cells[row, 1].Value.ToString();

                                    CategoryTemp category = new CategoryTemp()
                                    {
                                        CategoryName = CategoryName
                                    };
                                    if(category.CategoryName.Length > 4) 
                                    { 
                                    _db.ExcelUpdate.Add(category);
                                    _db.SaveChanges();
                                    }
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }

            return View();
        }
        [HttpGet]
        public IActionResult Create(CategoryTemp excel)
        {
            CategoryViewModel category = new CategoryViewModel();
            category.CId = excel.Id;
            category.CategoryName = excel.CategoryName;
            return View(category);
        }
        [HttpPost]
        public IActionResult Create(AddCategoryViewModel category)
        {
            var response = _categoryService.AddCategory(category);
            if (response.Status == "00") 
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }
      
    }
}
