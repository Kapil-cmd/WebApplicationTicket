using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Permission
    {
        public string PermissionId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<RolePermission> Roles { get; set; }
    }
}
