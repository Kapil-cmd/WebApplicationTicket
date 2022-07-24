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
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(TicketingContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Permission = new PermissionRepository(_db);
            Role = new RoleRepository(_db);
            Ticket = new TicketRepository(_db);
            User = new UserRepository(_db);
            Category = new CategoryRepository(_db);
        }
        public ICategoryRepository Category { get; private set; }

        public TicketingContext _db { get; private set; }
        public IPermissionRepository Permission { get; private set; }
        public IRoleRepository Role { get; private set; }
        public ITicketRepository Ticket { get; private set; }
        public IUserRepository User { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
