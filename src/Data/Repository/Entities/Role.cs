using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set;}
        public virtual IEnumerable<UserRole> Users { get; set; }
        public virtual IEnumerable<RolePermission> Permissions { get; set; }
    }
}
