﻿using Common.ViewModels.ValidationModel;
using Microsoft.AspNetCore.Mvc;
using Repository.Repos.Work;
using Services.BL;

namespace Web.Controllers
{
    public class FieldValidateController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFieldValidationService _fieldService;
        public FieldValidateController(IUnitOfWork unitOfWork, IFieldValidationService fieldService)
        {
            _unitOfWork = unitOfWork;
            _fieldService = fieldService;
        }

        public IActionResult Index()
        {
            var validate = _unitOfWork._db.Field.ToList();
            return View(validate);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
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
    }
}