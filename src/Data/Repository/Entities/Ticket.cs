using Common.Enums;
using Repository.Entities;

namespace Repository.Entites
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public string TicketDetails { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public string AssignedTo { get; set; }
        public string CategoryName { get; set; }
        public int? CategoryId { get; set; }
        public StatusEnum Status { get; set; }
        public string ImageName { get; set; }
        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
        public List<Category> Categories { get; set; }
        public virtual IEnumerable<UserTicket> Users { get; set; }
    }
}

