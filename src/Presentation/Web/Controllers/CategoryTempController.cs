using Common.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Repository;
using Repository.Entites;
using Repository.Entities;
using Repository.Repos.Work;
using Services.BL;
using System.Security.Claims;

namespace Web.Controllers
{
    public class CategoryTempController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly TicketingContext _db;
        private readonly ICategoryservice _categoryService;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryTempController(IWebHostEnvironment environment,TicketingContext db,ICategoryservice categoryservice,IUnitOfWork unitOfWork)
        {
            _webHostEnvironment = environment;
            _db = db;
            _categoryService = categoryservice;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var company = _db.CategoryTemp.ToList();
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


                            for (var row = 2; row <= rowCount; row++)
                            {
                                try
                                {
                                    var CategoryName = worksheet.Cells[row, 1].Value.ToString();

                                    CategoryTemp category = new CategoryTemp()
                                    {
                                        CategoryName = CategoryName
                                    };
                                    if (_unitOfWork._db.CategoryTemp.Any(x => x.CategoryName == category.CategoryName))
                                    {
                                        return View(Index);
                                    }
                                    else
                                    {
                                        if (category.CategoryName.Length > 3 && category.CategoryName.Take(3).All(char.IsLetter))
                                        {
                                            _db.CategoryTemp.Add(category);
                                            _db.SaveChanges();
                                        }
                                    }
                                }
                                catch (Exception ex)
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

            return View(Index);
        }
        [HttpGet]
        public IActionResult Send()
        {
            Category category = new Category();
            var excel = _db.CategoryTemp.ToList();
            var nameClaim = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            foreach(var categoryTemp in excel)
            {
                if (_unitOfWork.CategoryRepository.Any(x => x.CategoryName == categoryTemp.CategoryName))
                {
                    return View(Index);
                }
                else
                {
                    _db.Category.Add(new Repository.Entites.Category()
                    {
                        CategoryName = categoryTemp.CategoryName,
                        CreatedDateTime = DateTime.Now,
                        CreatedBy = nameClaim,
                    });
                    _db.SaveChanges();
                }
            }
            return View(Index);
        }
      
    }
}
