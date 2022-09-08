using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Entities;
using Repository.Repos.Work;

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
        public IActionResult Index(List<Company> students)
        {
            students = students == null ? new List<Company>() : students;
            return View(students);
        }
        [HttpPost]
        public  IActionResult Index(IFormFile file)
        {
            string fileName = $"{_webHostEnvironment.WebRootPath}\\files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            var companies = this.GetCompanyList(file.FileName);
            return Index(companies);
        }
        private List<Company> GetCompanyList(string fName)
        {
            List<Company> companies = new List<Company>();
            var fileName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using(var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        _db.Add(new Company()
                        {
                            Name = reader.GetString(0).ToString(),
                            NumberOfEmployee = reader.GetString(1).ToString(),
                            Address = reader.GetString(2).ToString(),
                            PhoneNumber = reader.GetString(3).ToString()
                        });
                    }
                }
            }
            return companies;
        }
      
    }
}
