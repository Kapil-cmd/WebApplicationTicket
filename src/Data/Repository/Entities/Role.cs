using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<UserRole> Users { get; set; }

        public virtual IEnumerable<RolePermission> Permissions { get; set; }
    }
}
