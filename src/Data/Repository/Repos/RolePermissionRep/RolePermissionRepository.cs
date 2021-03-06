using Repository.Entites;
using Repository.Repos.Reposi;

namespace Repository.Repos.RolePermissionRep
{
    public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(TicketingContext db) : base(db)
        {
        }
    }
    public interface IRolePermissionRepository : IRepository<RolePermission>
    {
    }
}
