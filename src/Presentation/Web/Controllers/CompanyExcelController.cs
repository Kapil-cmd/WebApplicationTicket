using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Repository;
using Repository.Entities;

namespace Web.Controllers
{
    public class CompanyExcelController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly TicketingContext _db;

        public CompanyExcelController(IWebHostEnvironment environment,TicketingContext db)
        {
            _webHostEnvironment = environment;
            _db = db;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var company = _db.Company.ToList();
            return View(company);
        }
        [HttpGet]
        public IActionResult GetExcel(List<Company> company)
        {
            company = company == null ? new List<Company>() : company;
            return View(company);
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

                    List<Company> companies = new List<Company>();
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
                                    var Name  = worksheet.Cells[row, 1].Value.ToString();
                                    var NumberOfEmployee = worksheet.Cells[row, 2].Value.ToString();
                                    var Address = worksheet.Cells[row, 3].Value.ToString();
                                    var PhoneNumber = worksheet.Cells[row, 4].Value.ToString();

                                    Company company = new Company()
                                    {
                                        Name = Name,
                                        NumberOfEmployee = NumberOfEmployee,
                                        Address = Address,
                                        PhoneNumber = PhoneNumber,
                                    };

                                    _db.Company.Add(company);
                                    _db.SaveChanges();
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
      
    }
}
