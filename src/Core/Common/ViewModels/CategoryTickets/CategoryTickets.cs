using Common.Enums;

namespace Common.ViewModels.CategoryTickets
{
    public class CategoryTickets
    {
        public List<ListCategory> category { get; set; }
        public List<ListTickets> Tickets { get; set; }
    }
    public class ListTickets
    {
        public string TicketId { get; set; } 
        public string TicketDetails { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public string AssignedTo { get; set; }
        public string CategoryName { get; set; }
        public StatusEnum Status { get; set; }
        public string ImageName { get; set; }
    }
    public class ListCategory
    {
        public string CId { get; set; }
        public string CategoryName { get; set; }
    }
}
