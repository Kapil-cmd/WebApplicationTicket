namespace Repository.Entites
{
    public class UserRole
    {

        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual User aUser{get;set;}
        public virtual Role aRole { get; set; }
        public List<Role> Roles { get; set; }
    }
}
