using Repository.Entites;

namespace Repository.Entities
{
    public class CategoryTicket
    {
        public string CategoryName { get; set; }
        public string TicketId { get; set; }

        public Category aCategory { get; set; }
        public Ticket aTicket { get; set; }
    }
}
