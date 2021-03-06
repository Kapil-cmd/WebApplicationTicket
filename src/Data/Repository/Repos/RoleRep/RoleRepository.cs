using Microsoft.EntityFrameworkCore;
using Repository.Entites;
using Repository.Repos.Reposi;

namespace Repository.Repos.RoleRep
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(TicketingContext db) : base(db)
        {
        }
    }

    public interface IRoleRepository : IRepository<Role>
    {
    }
}
