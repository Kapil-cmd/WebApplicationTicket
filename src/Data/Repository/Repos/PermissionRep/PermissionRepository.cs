using Repository.Entities;
using Repository.Repos.Reposi;

namespace Repository.Repos.PermissionRep
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(TicketingContext db) : base(db)
        {
        }
    }
}
