using Repository.Entities;
using Repository.Repos.BaseRepos;

namespace Repository.Repos.CategoryTicketRep
{
    public class CategoryTicketRepository : Repository<CategoryTicket>, ICategoryTicketRepository
    {
        public CategoryTicketRepository(TicketingContext db) : base(db)
        {
        }
    }
    public interface ICategoryTicketRepository : IRepository<CategoryTicket>
    {
    }
}
