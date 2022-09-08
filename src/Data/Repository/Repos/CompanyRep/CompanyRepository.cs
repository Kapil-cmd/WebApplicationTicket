using Repository.Entities;
using Repository.Repos.BaseRepos;

namespace Repository.Repos.CompanyRep
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(TicketingContext db) : base(db)
        {
        }
    }
    public interface ICompanyRepository : IRepository<Company>
    {
    }
}
