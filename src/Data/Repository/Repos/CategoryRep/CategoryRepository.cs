using Repository.Entities;
using Repository.Repos.Reposi;

namespace Repository.Repos.CategoryRep
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(TicketingContext db) : base(db)
        {
        }
    }
}
