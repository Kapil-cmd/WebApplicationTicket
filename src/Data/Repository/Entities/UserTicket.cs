using Repository.Entites;

namespace Repository.Entities
{
    public class UserTicket
    {
        public string TicketId { get; set; }
        public string UserId { get; set; }

        public virtual User aUser { get; set; }
        public virtual Ticket aTicket { get; set; }
    }
}
