namespace Repository.Entites
{
    public class Permission
    {
        public string PermissionId { get; set; }
        public string Name { get; set; } // user friendly name
        public string ParentPermissionId { get; set; } // Parent Id Of the permission
        public string Group { get; set; }
        public virtual ICollection<RolePermission> Roles { get; set; }
    }
}
