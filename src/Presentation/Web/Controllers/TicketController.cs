using Common.ViewModels.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entites;
using Repository.Repos.Work;
using Services.BL;

namespace demo.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketService _ticketService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TicketController(IUnitOfWork unitOfWork, ITicketService ticketService, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _ticketService = ticketService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var ticketList = _unitOfWork._db.Tickets.ToList();
            return View(ticketList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            AddTicketViewModel model = new AddTicketViewModel();
            var category = _unitOfWork._db.Category.ToList();
            if (category != null)
            {
                if (category.Count() > 0)
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
            if (ticket.Imagefile != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(ticket.Imagefile.FileName);
                string extension = Path.GetExtension(ticket.Imagefile.FileName);
                ticket.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image", fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await ticket.Imagefile.CopyToAsync(filestream);
                }
            }

            var response = _ticketService.AddTicket(ticket);
            if (response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(ticket);
            }
        }
        [HttpGet]
        public IActionResult EditTicket(string TicketId)
        {

            var ticket = _unitOfWork._db.Tickets.FirstOrDefault(x => x.TicketId == TicketId);
            if(ticket == null)
            {
                return NotFound();
            }
            EditTicketViewmodel model = new EditTicketViewmodel();
            model.TicketDetails = ticket.TicketDetails;
            var user = _unitOfWork._db.Users.ToList();
            if (user != null) 
            {
                if (user.Count() > 0)
                {
                    model.Users = user.Select(x => new ListUser()
                    {
                        Id = x.Id,
                        UserName = x.UserName
                    }).ToList();
                }

            }
            return View(model);
        }

        [HttpPost]
        public IActionResult EditTicket(EditTicketViewmodel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            var response = _ticketService.EditTicket(model);
            if (response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult DetailTicket(string ticketId)
        {
            if (ticketId == null)
            {
                return NotFound();
            }
            var ticket = _unitOfWork.Ticket.GetFirstOrDefault(x => x.TicketId == ticketId);
            if (ticket == null)
            {
                return NotFound();
            }
            else
            {
                return View(ticket);
            }
        }
        [HttpGet]
        public IActionResult DeleteTicket(string TicketId)
        {
            if(TicketId == null)
            {
                return NotFound();
            }
            var deleteTicket = _unitOfWork._db.Tickets.Find(TicketId);
            if(deleteTicket == null)
            {
                return NotFound();
            }
            return View(deleteTicket);
        }
        [HttpPost,ActionName("DeleteTicket")]
        public IActionResult DeletePost(Ticket model)
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
    }
}
