using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class RolePermission
    {

        public string PermissionId { get; set; }
        public string RoleId { get; set; }

        public virtual Permission aPermission { get; set; }
        public virtual Role aRole { get; set; }
    }
}
