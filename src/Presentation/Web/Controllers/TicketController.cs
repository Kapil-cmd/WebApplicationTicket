using Common.ViewModels.Tickets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Entites;
using Repository.Entities;
using Repository.Repos.Work;
using Services.BL;

namespace demo.Controllers
{

    public class TicketController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TicketingContext _db;
        private readonly ITicketService _ticketService;
        public TicketController(IUnitOfWork unitOfWork,TicketingContext db, ITicketService ticketService)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _ticketService = ticketService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            List<Category> category =  _db.Category.ToList();
            if (category == null)
            {
                return NotFound();
            }
            Ticket model = new Ticket
            {
                Categories = category,
            };
            return View(model);

        }
        [HttpPost]
        public IActionResult Create(AddTicketViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var response = _ticketService.AddTicket(model);
            if(response.Status == "00")
            {
                model.Status = Common.Enums.StatusEnum.Pending;
                //if (model.Imagefile != null)
                //{

                //    //var wwwRootPath = Directory.GetCurrentDirectory();

                //    string wwwRootPath = _webhostEnvironment.WebRootPath;
                //    string fileName = Path.GetFileNameWithoutExtension(model.Imagefile.FileName);
                //    string extension = Path.GetExtension(model.Imagefile.FileName);
                //    model.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                //    string path = Path.Combine(wwwRootPath + "/Image", fileName);
                //    using (var fileStream = new FileStream(path, FileMode.Create))
                //    {
                //        model.Imagefile.CopyToAsync(fileStream);
                //    }
                //}
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult EditTicket(int TicketId)
        {

            var ticketFromDb = _unitOfWork._db.Tickets.Find(TicketId);
            List<User> objUserList =  _unitOfWork._db.Users.ToList();

            var aTicket = new UserTicket
            {
                Users = objUserList,
                aTicket = ticketFromDb
            };
            return View();
        }

        [HttpPost]
        public IActionResult EditTicket(EditTicketViewmodel model)
        {
            if(!ModelState.IsValid)
            {
                return View("Index");
            }
            var response = _ticketService.EditTicket(model);
            if(response.Status =="00")
            {
                model.Status = Common.Enums.StatusEnum.InProcess;
                return RedirectToAction("Ticket");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult DetailTicket(int ticketId)
        {
            var response = _ticketService.TicketDetails(ticketId);
            if(response.Status == "00")
            {
                return RedirectToAction("Details");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult DeleteTicket(int TicketId)
        {
            var deleteTicket = _unitOfWork._db.Tickets.Find(TicketId);
            return View(deleteTicket);
        }
        [HttpPost]
        public IActionResult DeleteTicket(TicketViewModel model)
        {
            var response = _ticketService.DeleteTicket(model);
            if (response.Status == "00") 
            {
                return RedirectToAction("DeleteTicket");
            }
            else
            {
                return View(model);
            }
        }
        
        [HttpPost]
        public IActionResult AssignTicketToDeveloper(int ticketId,string userId)
        {
            var model = new EditTicketViewmodel();

            var response = _ticketService.AssignTicketToDeveloper(ticketId,userId);
            if(response.Status =="00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
            
        }
    }
}
