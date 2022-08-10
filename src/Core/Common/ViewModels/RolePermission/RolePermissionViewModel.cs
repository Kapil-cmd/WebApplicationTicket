namespace Common.ViewModels.RolePermission
{
    public class RolePermissionViewModel
    {
        public string RoleId { get; set; }
        public string PermissionId { get; set; }

        public bool IsSelected { get; set; }

        public List<PermissionList> Permissions { get; set; }
    }
    public class PermissionList
    {
        public string Id { get; set; }
        public string PermissionName { get; set; }
    }
}
