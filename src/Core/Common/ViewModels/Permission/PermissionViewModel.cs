using System.ComponentModel.DataAnnotations;

namespace Common.ViewModels.Permission
{
    public class PermissionViewModel
    {
        public string PermissionId { get; set; }
        public string Name { get; set; } 
        public string ParentPermissionId { get; set; } 

    }
    public class AddPermission
    {
        [Key]
        public string PermissionId { get; set; }
        public string Name { get; set; }
        public string ModuleCode { get; set; }
        public string Group { get; set; }
        public string ParentPermissionId { get; set; } 
    }
    public class EditPermission
    {
        public string PermissionId { get; set; }
        public string Name { get; set; } 
        public string ParentPermissionId { get; set; } 

    }
}
