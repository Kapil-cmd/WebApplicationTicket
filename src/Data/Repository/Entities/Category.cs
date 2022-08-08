using Common.ViewModels.Tickets;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class Category
    {
        public string CId { get; set; } = Guid.NewGuid().ToString();
        public string CategoryName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public virtual User User { get; set; }
        public virtual IEnumerable<ListCategory> listCategories { get; set; }
        public virtual IEnumerable<Ticket> Tickets { get; set; }
    }
}
