using Microsoft.AspNetCore.Http;
using Repository.Repos.CategoryRep;
using Repository.Repos.PermissionRep;
using Repository.Repos.RolePermissionRep;
using Repository.Repos.RoleRep;
using Repository.Repos.TicketCategoryRep;
using Repository.Repos.TicketRep;
using Repository.Repos.TicketUserRep;
using Repository.Repos.UserRep;
using Repository.Repos.UsersRep;

namespace Repository.Repos.Work
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(TicketingContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            Role = new RoleRepository(_db);
            Ticket = new TicketRepository(_db);
            CategoryRepository = new CategoryRepository(_db);
            UserRoleRepository = new UserRoleRepository(_db);
            UserRepository = new UserRepository(_db);
            Permission = new PermissionRepository(_db);
            RolePermissionRepository = new RolePermissionRepository(_db);
            UserTicketRepository = new UserTicketRepository(_db);
            TicketCategoryRepository = new TicketCategoryRepository(_db);
        }
        public IHttpContextAccessor _httpContextAccessor { get; private set; }
        public TicketingContext _db { get; private set; }
        public IRoleRepository Role { get; private set; }
        public ITicketRepository Ticket { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IUserRoleRepository UserRoleRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public IPermissionRepository Permission { get; private set; }
        public IRolePermissionRepository RolePermissionRepository { get; private set; }
        public IUserTicketRepository UserTicketRepository { get; private set; }
        public ITicketCategoryRepository TicketCategoryRepository { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
