using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class UserRole
    {

        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual User aUser{get;set;}
        public virtual Role aRole { get; set; }
    }
}
