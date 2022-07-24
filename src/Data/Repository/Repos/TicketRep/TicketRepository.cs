using Repository.Entities;
using Repository.Repos.Reposi;
using System.Linq.Expressions;

namespace Repository.Repos.TicketRep
{
    public class TicketRepository : Repository<Ticket>,ITicketRepository
    {
        public TicketRepository(TicketingContext db) : base(db)
        {
        }

       
    }
}
