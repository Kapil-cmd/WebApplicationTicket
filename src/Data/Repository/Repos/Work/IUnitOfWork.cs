using Microsoft.AspNetCore.Http;
using Repository.Repos.CategoryRep;
using Repository.Repos.CompanyRep;
using Repository.Repos.PermissionRep;
using Repository.Repos.RolePermissionRep;
using Repository.Repos.RoleRep;
using Repository.Repos.TicketRep;
using Repository.Repos.TicketUserRep;
using Repository.Repos.UserRep;
using Repository.Repos.UsersRep;

namespace Repository.Repos.Work
{
    public interface IUnitOfWork
    {
        public TicketingContext _db { get; }

        ICategoryRepository CategoryRepository { get; }
        IHttpContextAccessor _httpContextAccessor { get; }
        IRoleRepository Role { get; }
        ITicketRepository Ticket { get; }
        IUserRepository UserRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IPermissionRepository Permission { get; }
        IRolePermissionRepository RolePermissionRepository { get; }
        IUserTicketRepository UserTicketRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        void Save();
    }
}
