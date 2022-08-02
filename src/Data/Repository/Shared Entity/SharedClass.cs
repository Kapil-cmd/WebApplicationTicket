using Common.ViewModels.Tickets;
using Repository.Entites;

namespace Repository.Shared_Entity
{
    public class SharedClass
    {
        public List<AddTicketViewModel> Tickets { get; set; }
        public List<Category> Categories { get; set; }
    }
}
