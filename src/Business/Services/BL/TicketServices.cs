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
                    CreatedDateTime = DateTime.UtcNow,
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

        public BaseResponseModel<string> CloseTicket(CloseTicket Ticket)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var ticket = _unitOfWork._db.Tickets.FirstOrDefault(x => x.TicketId == Ticket.TicketId );
                if(ticket == null)
                {
                    response.Status = "404";
                    response.Message = "Ticket Not found";
                    return response;
                }
                else
                {
                    ticket.Status = Common.Enums.StatusEnum.Completed;
                    _unitOfWork._db.Tickets.Update(ticket);
                    _unitOfWork._db.SaveChanges();

                    
                }
                response.Status = "00";
                response.Message = "Ticket closed sucessfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Exception occurred :" + ex.Message;
                return response;
            }
        }

        public BaseResponseModel<string> DeleteTicket(Ticket Ticket)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var ticket = _unitOfWork._db.Tickets.FirstOrDefault(x => x.TicketId == Ticket.TicketId);
                if(ticket == null)
                {
                    response.Status = "404";
                    response.Message = "Ticket not found";
                    return response;
                }
                _unitOfWork._db.Tickets.Remove(ticket);
                _unitOfWork._db.SaveChanges();
                
                
                response.Status = "00";
                response.Message = "Ticket deleted sucessully";
                return response;
            }catch(Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occurred :" + ex.Message;
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

                #region TicketUpdate
                var nameClaim = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                ticket.TicketDetails = Ticket.TicketDetails;
                ticket.ModifiedDateTime = DateTime.UtcNow;
                ticket.ModifiedBy = Ticket.ModifiedBy = nameClaim;
                ticket.Status = Common.Enums.StatusEnum.InProcess;
                ticket.AssignedTo = Ticket.AssignedTo;

                _unitOfWork._db.Tickets.Update(ticket);
                _unitOfWork._db.SaveChanges();
                #endregion
                #region TicketAssign
                if (_unitOfWork.UserTicketRepository.Any(x => x.UserName == Ticket.AssignedTo && x.TicketId == Ticket.TicketId))
                {
                    response.Status = "444";
                    response.Message = "This ticket is already assigned to this user";
                    return response;
                }

                _unitOfWork._db.UserTickets.Add(new Repository.Entities.UserTicket()
                {
                    TicketId = Ticket.TicketId,
                    UserName = Ticket.AssignedTo
                });

                _unitOfWork._db.SaveChanges();
                #endregion
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
    }

        public interface ITicketService
    {
        BaseResponseModel<string> AddTicket(AddTicketViewModel Ticket);
        BaseResponseModel<string> EditTicket(EditTicketViewmodel Ticket);
        BaseResponseModel<string> DeleteTicket(Ticket Ticket);
        BaseResponseModel<string> TicketDetails(string ticketId);
        BaseResponseModel<string> CloseTicket(CloseTicket Ticket);
    }
}

