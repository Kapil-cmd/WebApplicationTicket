using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class UserRole
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }

        public virtual Role aRole { get; set; }
        public virtual User aUser { get; set; }
    }
}
