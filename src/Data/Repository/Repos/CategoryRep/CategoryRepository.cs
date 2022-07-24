using Microsoft.EntityFrameworkCore;
using Repository.Entites;
using Repository.Repos.Reposi;

namespace Repository.Repos.CategoryRep
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(TicketingContext db) : base(db)
        {
        }
    }

    public interface ICategoryRepository : IRepository<Category>
    {
    }
}
