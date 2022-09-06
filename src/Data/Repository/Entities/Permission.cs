namespace Repository.Entites
{
    public class Permission
    {
        public string PermissionId { get; set; }
        public string Slug { get; set; } // user friendly name
        public string ParentId { get; set; } // Parent Id Of the permission
        public string Group { get; set; }//remember it as name category
        public string MenuName { get; set; }
        public virtual ICollection<RolePermission> Roles { get; set; }
    }
}
