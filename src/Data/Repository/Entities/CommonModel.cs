using Repository.Entites;

namespace Repository.Entities
{
    public class CommonModel
    {
        public UserRole UserRole { get; set; }
        public User user { get; set; }
        public Role role { get; set; }

        public Ticket Tickets { get; set; }
        public UserTicket UTicket { get; set; }
    }
}
