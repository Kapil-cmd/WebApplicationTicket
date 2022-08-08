using Repository.Entites;
using Repository.Repos.BaseRepos;

namespace Repository.Repos.UserRep
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(TicketingContext db) : base(db)
        {
        }
    }

    public interface IUserRoleRepository : IRepository<UserRole>
    {
    }
}
