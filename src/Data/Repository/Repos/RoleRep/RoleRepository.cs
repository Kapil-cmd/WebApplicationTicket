using Repository.Entities;
using Repository.Repos.Reposi;

namespace Repository.Repos.RoleRep
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(TicketingContext db) : base(db)
        {
        }
    }
}
