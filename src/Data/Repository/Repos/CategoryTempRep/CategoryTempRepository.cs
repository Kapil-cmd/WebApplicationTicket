using Repository.Entities;
using Repository.Repos.BaseRepos;

namespace Repository.Repos.ExcelRep
{
    public class CategoryTempRepository : Repository<CategoryTemp>, ICategoryTempRepository
    {
        public CategoryTempRepository(TicketingContext db) : base(db)
        {
        }
    }
    public interface ICategoryTempRepository : IRepository<CategoryTemp>
    {
    } 
}
