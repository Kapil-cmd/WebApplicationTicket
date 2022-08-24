using Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class Permission
    {
        public string PermissionId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool Permitted { get; set; }
        public virtual ICollection<RolePermission> Roles { get; set; }

      
    }
}
