using Repository.Entities;
using Repository.Repos.BaseRepos;

namespace Repository.Repos.TicketUserRep
{
    public class UserTicketRepository : Repository<UserTicket>, IUserTicketRepository
    {
        public UserTicketRepository(TicketingContext db) : base(db)
        {
        }
    }
    public interface IUserTicketRepository : IRepository<UserTicket>
    {
    }
}
