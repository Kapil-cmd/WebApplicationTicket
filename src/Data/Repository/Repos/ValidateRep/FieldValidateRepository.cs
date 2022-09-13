using Repository.Entities;
using Repository.Repos.BaseRepos;

namespace Repository.Repos.ValidateRep
{
    public class FieldValidateRepository : Repository<FieldValidation>,IFieldValidateRepository
    {
        public FieldValidateRepository(TicketingContext db) : base(db)
        {
        }
    }
    public interface IFieldValidateRepository : IRepository<FieldValidation>
    {
    }
}
