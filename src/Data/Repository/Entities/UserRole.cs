using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class UserRole
    {

        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }

        public virtual User aUser{get;set;}
        public virtual Role aRole { get; set; }
        //[NotMapped]
        //public List<Role> Roles { get; set; }
    }
}
