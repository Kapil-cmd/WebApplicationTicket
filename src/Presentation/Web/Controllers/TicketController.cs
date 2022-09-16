using Common.ViewModels.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Repository.Entites;
using Repository.Repos.Work;
using Services.BL;
using Services.CustomFilter;
using System.Security.Claims;
using X.PagedList;

namespace demo.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketService _ticketService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IToastNotification _toastNotification;
        
        public TicketController(IUnitOfWork unitOfWork,ITicketService ticketService, IWebHostEnvironment webHostEnvironment,IToastNotification toastNotification)
        {
            _unitOfWork = unitOfWork;
            _ticketService = ticketService;
            _webHostEnvironment = webHostEnvironment;
            _toastNotification = toastNotification;
        }
        [PermissionFilter("Admin&Ticket&View_Ticket")]
        public IActionResult Index(string Sorting_Order,string Search_Data,string Filter_Value,int? Page_No)
        {
            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Name_Description" : "";
            ViewBag.SortingCName = String.IsNullOrEmpty(Sorting_Order) ? "Category_Description" : "";
            ViewBag.SortingDate = Sorting_Order == "Created_Date" ? "Date_Description" : "Date";
            if (Search_Data != null)
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }
            ViewBag.FilterValue = Search_Data;

            var tickets = from ticket in _unitOfWork._db.Tickets select ticket;
            
            if(!String.IsNullOrEmpty(Search_Data))
            {
                tickets = tickets.Where(ticket => ticket.CreatedBy.ToUpper().Contains(Search_Data.ToUpper()));
                
            }
            switch (Sorting_Order)
            {
                case "Name_Description":
                    tickets = tickets.OrderByDescending(ticket => ticket.CreatedBy);
                break;
                case "Category_Description":
                    tickets = tickets.OrderByDescending(ticket => ticket.CategoryName);
                break;

                case "Date_Description":
                    tickets = tickets.OrderByDescending(ticket => ticket.CreatedDateTime);
                break;
                default:
                    tickets = tickets.OrderBy(ticket => ticket.CreatedDateTime);
                break;
            }
            int Size_Of_Page = 4;
            int No_Of_page = (Page_No ?? 1);
            return View(tickets.ToPagedList(No_Of_page, Size_Of_Page));

        }
        public IActionResult UserIndex()
        {
            var model = _unitOfWork._db.Tickets.Where(a => a.User.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).OrderByDescending(x => x.CreatedDateTime).ToList();
            return View(model);
        }
        [PermissionFilter("Admin&Ticket&DevView_Ticket")]
        public IActionResult DeveloperIndex()
        {
            var user = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var model = _unitOfWork._db.Tickets.Where(x => x.AssignedTo == user).OrderByDescending(x => x.ModifiedDateTime).ToList();
            return View(model);
        }
        [HttpGet]
        [PermissionFilter("Admin&Ticket&Create_Ticket")]
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
        [PermissionFilter("Admin&Ticket&Create_Ticket")]
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
                _toastNotification.AddSuccessToastMessage("Ticket created successfully");
                return RedirectToAction("Index");
            }
            else
            {
                _toastNotification.AddWarningToastMessage("Unable to create ticket");
                return View(ticket);
            }
        }
        [HttpGet]
        [PermissionFilter("Admin&Ticket&Edit_Ticket")]
        public IActionResult EditTicket(string TicketId)
        {

            var ticket = _unitOfWork._db.Tickets.FirstOrDefault(x => x.TicketId == TicketId);
            if(ticket == null)
            {
                return NotFound();
            }
            EditTicketViewmodel model = new EditTicketViewmodel();
            model.TicketDetails = ticket.TicketDetails;
            var user = _unitOfWork._db.UserRoles.Include(x => x.aUser).Include(x => x.aUser.Roles).Where(x => x.aUser.Roles.Any(x => x.RoleName =="Developer"));
            
            if (user != null) 
            {
                if (user.Count() > 0)
                {
                    model.Users = user.Select(x => new ListUser()
                    {
                        UserName = x.UserName
                    }).ToList();
                }

            }
            return View(model);
        }

        [HttpPost]
        [PermissionFilter("Admin&Ticket&Edit_Ticket")]
        public IActionResult EditTicket(EditTicketViewmodel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            var response = _ticketService.EditTicket(model);
            if (response.Status == "00")
            {
                _toastNotification.AddSuccessToastMessage("Ticket Edited sucessfully");
                return RedirectToAction("Index");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Unable to edit ticket");
                return View(model);
            }
        }
        [HttpGet]
        [PermissionFilter("Admin&Ticket&Detail_Ticket")]
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
        [PermissionFilter("Admin&Ticket&Delete_Ticket")]
        public IActionResult Delete(string? TicketId)
        {
            if(TicketId == null)
            {
                return NotFound();
            }
            var ticket = _unitOfWork._db.Tickets.FirstOrDefault(x => x.TicketId == TicketId);
            if(ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }
        [HttpPost]
        [PermissionFilter("Admin&Ticket&Delete_Ticket")]
        public IActionResult Delete(Ticket ticket)
        {
            var response = _ticketService.DeleteTicket(ticket);
            if(response.Status == "00")
            {
                _toastNotification.AddSuccessToastMessage("Ticket deleted sucessfully");
                return RedirectToAction("Index");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Unable to delete ticket");
                return View(ticket);
            }
        }
        [HttpGet]
        public IActionResult CloseTicket(CloseTicket ticket)
        {
            var response = _ticketService.CloseTicket(ticket);
            if(response.Status == "00")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(ticket);
            }
        }
    }
}
