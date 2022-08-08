namespace Repository.Entites
{
    public class RolePermission
    {

        public string PermissionId { get; set; }
        public string RoleId { get; set; }

        public virtual Permission aPermission { get; set; }
        public virtual Role aRole { get; set; }
        //public IEnumerable<Permission> Permissions { get; set; }
    }
}
