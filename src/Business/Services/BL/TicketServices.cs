using Common.ViewModels.BaseModel;
using Common.ViewModels.Tickets;
using Repository.Entites;
using Repository.Entities;
using Repository.Repos.Work;
using System.Security.Claims;

namespace Services.BL
{
    public class TicketServices : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TicketServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BaseResponseModel<int> AddTicket(AddTicketViewModel Ticket)
        {
            var response = new BaseResponseModel<int>();
            try
            {
                var nameClaim = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                var categoryId = _unitOfWork._db.Category.FindAsync(_unitOfWork._db.Category);
                Ticket.CreatedBy = nameClaim;
                Ticket.CreatedDateTime = DateTime.Now;

                if (Ticket.Imagefile != null)
                {

                    var wwwRootPath = Directory.GetCurrentDirectory();

                    //string wwwRootPath = _unitOfWork._webHostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(Ticket.Imagefile.FileName);
                    string extension = Path.GetExtension(Ticket.Imagefile.FileName);
                    Ticket.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Image", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        Ticket.Imagefile.CopyToAsync(fileStream);
                    }
                }

                _unitOfWork._db.Tickets.Add(new Repository.Entites.Ticket()
                {
                    TicketDetails = Ticket.TicketDetails,
                    CategoryName = Ticket.CategoryName,
                    CreatedBy = Ticket.CreatedBy,
                    CreatedDateTime = Ticket.CreatedDateTime,
                    ImageName = Ticket.ImageName,
                    Status = Ticket.Status,
                });

                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "Ticket added sucessfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured" + ex.Message;
                return response;
            }
        }
        public BaseResponseModel<int> EditTicket(EditTicketViewmodel Ticket)
        {
            var response = new BaseResponseModel<int>();
            try
            {
                var ticket = _unitOfWork._db.Tickets.FirstOrDefault(x => x.TicketId == Ticket.TicketId);
                if (ticket == null)
                {
                    response.Status = "100";
                    response.Message = "Ticket not Found";
                    return response;
                }
                ticket.TicketDetails = Ticket.TicketDetails;
                ticket.ModifiedBy = Ticket.ModifiedBy;
                ticket.ModifiedDateTime = Ticket.ModifiedDateTime;
                ticket.Status = Ticket.Status;
                ticket.AssignedTo = Ticket.AssignedTo;


                var nameClaim = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                Ticket.ModifiedBy = nameClaim;
                Ticket.ModifiedDateTime = DateTime.Now;

                _unitOfWork._db.Tickets.Update(ticket);
                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "Ticket Updated Sucessfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured: " + ex.Message;
                return response;
            }
        }
        public BaseResponseModel<int> DeleteTicket(TicketViewModel DeleteTicket)
        {
            var response = new BaseResponseModel<int>();
            try
            {
                var Ticket = _unitOfWork._db.Tickets.FirstOrDefault(x => x.TicketId == DeleteTicket.TicketId);
                if (Ticket == null)
                {
                    response.Status = "100";
                    response.Message = "Ticket not found";
                    return response;
                }
                Ticket.TicketId = DeleteTicket.TicketId;
                Ticket.TicketDetails = DeleteTicket.TicketDetails;
                Ticket.CreatedBy = DeleteTicket.CreatedBy;
                Ticket.CreatedDateTime = DeleteTicket.CreatedDateTime;
                Ticket.ModifiedBy = DeleteTicket.ModifiedBy;
                Ticket.ModifiedDateTime = DeleteTicket.ModifiedDateTime;
                Ticket.AssignedTo = DeleteTicket.AssignedTo;
                Ticket.CategoryName = DeleteTicket.CategoryName;
                Ticket.Status = DeleteTicket.Status;
                Ticket.ImageName = DeleteTicket.ImageName;

                _unitOfWork._db.Tickets.Remove(Ticket);
                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "Ticket deleted sucessfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured: " + ex.Message;
                return response;
            }
        }
        public BaseResponseModel<int> TicketDetails(int ticketId)
        {
            var response = new BaseResponseModel<int>();
            try
            {
                var Ticket = _unitOfWork._db.Tickets.Find(ticketId);
                if (Ticket == null)
                {
                    response.Status = "100";
                    response.Message = "Ticket not found";
                    return response;
                }
                response.Status = "00";
                response.Message = "Ticket displayed";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured: " + ex.Message;
                return response;
            }

        }
        public BaseResponseModel<int> AssignTicketToDeveloper(int ticketId, string userId)
        {
            var response = new BaseResponseModel<int>();
            try
            {
                if (_unitOfWork.UserTicketRepository.Any(x => x.UserId == userId && x.TicketId == ticketId))
                {
                    response.Status = "444";
                    response.Message = "This ticket is already assigned to this user";
                    return response;
                }
                _unitOfWork.UserTicketRepository.Add(new Repository.Entities.UserTicket()
                {
                    TicketId = ticketId,
                    UserId = userId
                });
                _unitOfWork.Save();
                response.Status = "00";
                response.Message = "Ticket assigned successfully";

                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occurred:" + ex.Message;

                return response;
            }
        }
    }

        public interface ITicketService
    {
        BaseResponseModel<int> AddTicket(AddTicketViewModel Ticket);
        BaseResponseModel<int> EditTicket(EditTicketViewmodel Ticket);
        BaseResponseModel<int> DeleteTicket (TicketViewModel Ticket);
        BaseResponseModel<int> TicketDetails(int ticketId);
        BaseResponseModel<int> AssignTicketToDeveloper(int ticketId, string userId);
    }
}

