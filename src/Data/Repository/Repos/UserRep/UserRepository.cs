
using Repository.Entities;
using Repository.Repos.Reposi;

namespace Repository.Repos.UserRep
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(TicketingContext db) : base(db)
        {
        }
    }
}
