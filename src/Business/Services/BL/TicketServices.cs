using Common.ViewModels.BaseModel;
using Common.ViewModels.Tickets;
using Repository.Entites;
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
        public BaseResponseModel<string> AddTicket(AddTicketViewModel Ticket)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var nameClaim = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                Ticket.Status = Common.Enums.StatusEnum.Pending;

                _unitOfWork._db.Tickets.Add(new Repository.Entites.Ticket()
                {
                    TicketDetails = Ticket.TicketDetails,
                    CategoryName = Ticket.CategoryName,
                    CreatedBy = Ticket.CreatedBy = nameClaim,
                    CreatedDateTime = DateTime.Now,
                    ImageName = Ticket.ImageName,
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
        public BaseResponseModel<string> EditTicket(EditTicketViewmodel Ticket)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var ticket = _unitOfWork._db.Tickets.FirstOrDefault(x => x.TicketId == Ticket.TicketId);
                if (ticket == null)
                {
                    response.Status = "100";
                    response.Message = "Ticket not Found";
                    return response;
                }
                var nameClaim = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                ticket.TicketDetails = Ticket.TicketDetails;
                ticket.ModifiedDateTime = DateTime.Now;
                ticket.ModifiedBy = Ticket.ModifiedBy = nameClaim;
                ticket.Status = Common.Enums.StatusEnum.InProcess;
                ticket.AssignedTo = Ticket.AssignedTo;

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
        public BaseResponseModel<string> DeleteTicket(Ticket model)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                //var Ticket = _unitOfWork._db.Tickets.FirstOrDefault(x => x.TicketId == model.TicketId);
                //if (Ticket == null)
                //{
                //    response.Status = "100";
                //    response.Message = "Ticket not found";
                //    return response;
                //}
               
                _unitOfWork._db.Tickets.Remove(model);
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
        public BaseResponseModel<string> TicketDetails(string ticketId)
        {
            var response = new BaseResponseModel<string>();
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
        public BaseResponseModel<string> AssignTicketToDeveloper(string ticketId, string userId)
        {
            var response = new BaseResponseModel<string>();
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
        BaseResponseModel<string> AddTicket(AddTicketViewModel Ticket);
        BaseResponseModel<string> EditTicket(EditTicketViewmodel Ticket);
        BaseResponseModel<string> DeleteTicket (Ticket model);
        BaseResponseModel<string> TicketDetails(string ticketId);
        BaseResponseModel<string> AssignTicketToDeveloper(string ticketId, string userId);
    }
}

