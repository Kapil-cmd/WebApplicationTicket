using Repository.Entites;

namespace Repository.Entities
{
    public class CategoryTicket
    {
        public string TicketId { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual Category aCategory { get; set; }
        public virtual Ticket aTicket { get; set; }
    }
}
