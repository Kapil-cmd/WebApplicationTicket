using System.ComponentModel.DataAnnotations;

namespace Services.BL
{
    public class PermissionViewModel
    {
        public string PermissionId { get; set; }
        public string Name { get; set; }
    }
    public class AddPermission
    {
        public string PermissionId { get; set; }
        [Required(ErrorMessage = "Permission is required"), MaxLength(25), Display(Name = "PermissionName")]
        public string Name { get; set; }
    }
    public class EditPermission
    {
        public string PermissionId { get; set; }
        [MaxLength(25), Display(Name = "PermissionName")]
        public String Name { get; set; }
    }
}
