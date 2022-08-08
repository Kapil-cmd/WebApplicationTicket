using Repository.Entites;
using Repository.Repos.BaseRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos.PermissionRep
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(TicketingContext db) : base(db)
        {
        }
    }
    public interface IPermissionRepository :IRepository<Permission>
{
}
}
