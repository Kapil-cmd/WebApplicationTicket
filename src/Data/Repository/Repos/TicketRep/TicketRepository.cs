using Repository.Entites;
using Repository.Repos.BaseRepos;

namespace Repository.Repos.TicketRep
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(TicketingContext db) : base(db)
        {
        }
    }
    public interface ITicketRepository : IRepository<Ticket>
    {
    }
}
