﻿using Repository.Entites;
using Repository.Repos.BaseRepos;

namespace Repository.Repos.UsersRep
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(TicketingContext db) : base(db)
        {
        }
    }
    public interface IUserRepository:IRepository<User>
    {

    }
}
