using Repository.Entities;
using Repository.Repos.BaseRepos;

namespace Repository.Repos.TicketCategoryRep
{
    public class TicketCategoryRepository : Repository<CategoryTicket>, ITicketCategoryRepository
    {
        public TicketCategoryRepository(TicketingContext db) : base(db)
        {
        }
    }
    public interface ITicketCategoryRepository : IRepository<CategoryTicket> 
    {
    }
}
