using Common.ViewModels.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Entites;
using Repository.Entities;
using Repository.Repos.Work;
using Services.BL;

namespace demo.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TicketingContext _db;
        private readonly ITicketService _ticketService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TicketController(IUnitOfWork unitOfWork,TicketingContext db, ITicketService ticketService,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _ticketService = ticketService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Ticket> objFromTickets = _unitOfWork._db.Tickets.ToList();
            return View(objFromTickets);
        }
        [HttpGet]
        public IActionResult Create()
        {
            AddTicketViewModel model = new AddTicketViewModel();
            var category = _unitOfWork._db.Category.ToList();
            if(category != null)
            {
                if(category.Count() > 0)
                {
                    model.categories = category.Select(x => new ListCategory()
                    {
                        CId = x.CId,
                        CategoryName = x.CategoryName
                    }).ToList();
                }
            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Create(AddTicketViewModel ticket)
        {
            if (!ModelState.IsValid)
            {
                return View(ticket);
            }
            if(ticket.Imagefile != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(ticket.Imagefile.FileName);
                string extension = Path.GetExtension(ticket.Imagefile.FileName);
                ticket.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image", fileName);
                using(var filestream = new FileStream(path, FileMode.Create))
                {
                    await ticket.Imagefile.CopyToAsync(filestream);
                }
            }
            var response = _ticketService.AddTicket(ticket);
            if(response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(ticket);
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
        public IActionResult DetailTicket(string ticketId)
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
        public IActionResult AssignTicketToDeveloper(string ticketId,string userId)
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
