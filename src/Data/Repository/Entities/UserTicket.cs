using Repository.Entites;

namespace Repository.Entities
{
    public class UserTicket
    {
        public int TicketId { get; set; }
        public string UserId { get; set; }

        public virtual User aUser { get; set; }
        public virtual Ticket aTicket { get; set; }
        public List<User> Users { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
