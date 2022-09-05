namespace Repository.Entites
{
    public class RolePermission
    {

        public string PermissionId { get; set; }
        public string RoleId { get; set; }
        public string Slug { get; set; } // user friendly name
        public string ParentPermissionId { get; set; } // Parent Id Of the permission
        public string Group { get; set; }//remember it as name category
        public virtual Permission aPermission { get; set; }
        public virtual Role aRole { get; set; }
    }
}
