using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Repository.Repos.Work;

namespace Web.Controllers
{
    public class CompanyExcelController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly UnitOfWork _unitOfWork;

        public CompanyExcelController(IWebHostEnvironment environment,UnitOfWork unitOfWork)
        {
            _webHostEnvironment = environment;
            _unitOfWork = unitOfWork;   
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
            var students = this.GetStudentList(file.FileName);
            return Index(students);
        }
        private List<Company> GetStudentList(string fName)
        {
            List<Company> students = new List<Company>();
            var fileName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "" + fName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using(var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        _unitOfWork._db.Add(new Company()
                        {
                            Name = reader.GetString(0).ToString(),
                            NumberOfEmployee = reader.GetDouble(1),
                            Address = reader.GetString(3).ToString(),
                            PhoneNumber = reader.GetDouble(4)
                        });
                    }
                }
            }
            return students;
        }
      
    }
}
