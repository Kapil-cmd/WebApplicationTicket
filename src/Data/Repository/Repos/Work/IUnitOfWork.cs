using Repository.Repos.CategoryRep;
using Repository.Repos.PermissionRep;
using Repository.Repos.RoleRep;
using Repository.Repos.TicketRep;
using Repository.Repos.UserRep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos.Work
{
    public interface IUnitOfWork
    {
        public TicketingContext _db { get; }

        ICategoryRepository Category { get; }

        IPermissionRepository Permission { get; }

        IRoleRepository Role { get; }

        ITicketRepository Ticket { get; }

        IUserRepository User { get; }

        void Save();
    }
}
