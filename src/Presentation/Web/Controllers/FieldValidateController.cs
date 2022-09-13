using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Entities;

namespace Web.Controllers
{
    public class FieldValidateController : Controller
    {
        private TicketingContext _db;
        public FieldValidateController(TicketingContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var validate = _db.Field.ToList();
            return View(validate);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(FieldValidation field)
        {
            if (ModelState.IsValid)
            {
                if (_db.Field.Any(x => x.Name == field.Name))
                {
                    return View(Index);
                }
                else
                {
                    _db.Field.Add(field);
                    _db.SaveChanges();
                    return View("Index");
                }
            }
            else
            {
                return View("Create");
            }   
        }
    }
}
