using Common.ViewModels.ValidationModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Repository.Repos.Work;
using Services.BL;
using Services.CustomFilter;

namespace Web.Controllers
{
    [Authorize]
    public class FieldValidateController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFieldValidationService _fieldService;
        public FieldValidateController(IUnitOfWork unitOfWork, IFieldValidationService fieldService)
        {
            _unitOfWork = unitOfWork;
            _fieldService = fieldService;
        }
        [PermissionFilter("Admin&Validate&View_Field")]
        public IActionResult Index()
        {
            var validate = _unitOfWork._db.Field.ToList();
            return View(validate);
        }
        [PermissionFilter("Admin&Validate&Create_Field")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [PermissionFilter("Admin&Validate&Create_Field")]
        [HttpPost]
        public IActionResult Create(AddValidationField field)
        {
            if (!ModelState.IsValid)
            {
                return View(field);
            }
            var response = _fieldService.AddValdation(field);
            if(response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(field);
            }
        }
        [PermissionFilter("Admin&Validate&Edit_Field")]
        [HttpGet]
        public IActionResult Edit(string? Id)
        {
            var validate = _unitOfWork._db.Field.FirstOrDefault(x => x.Id == Id);
            if(validate == null)
            {
                return NotFound();
            }
            EditValidationField field = new EditValidationField();
            field.Id = validate.Id;
            field.Name = validate.Name;
            field.Length = validate.Length;

            return View(field);
        }
        [PermissionFilter("Admin&Validate&Edit_Field")]
        [HttpPost]
        public IActionResult Edit(EditValidationField validationField)
        {
            var response = _fieldService.EditValidation(validationField);
            if(response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(validationField);
            }
        }
        [HttpGet]
        public IActionResult deleteField(string Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            else
            {
                var field = _unitOfWork._db.Field.FirstOrDefault(x => x.Id == Id);
                if(field == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(field);
                }
            }
        }
        [HttpPost]
        public IActionResult deleteField(FieldValidation validation)
        {
            var response = _fieldService.DeleteValidation(validation);
            if(response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(validation);
            }
        }
    }
}
