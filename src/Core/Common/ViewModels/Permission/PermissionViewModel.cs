using System.ComponentModel.DataAnnotations;

namespace Common.ViewModels.Permission
{
    public class PermissionViewModel
    {
        public string PermissionId { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
       
    }
    public class AddPermission
    {
        [Key]
        public string PermissionId { get; set; }
        [Required(ErrorMessage = "Permission is required"), MaxLength(25), Display(Name = "PermissionName")]
        public string Name { get; set; }
        public string ActionName { get; set; }
        [Required(ErrorMessage = "ControllerName is required"), Display(Name = "Controller Name")]
        public string ControllerName { get; set; }
    }
    public class EditPermission
    {
        public string PermissionId { get; set; }
        [MaxLength(25), Display(Name = "PermissionName")]
        public String Name { get; set; }

        [Display(Name = "Controller Name")]
        public string ControllerName { get; set; }
        [Display(Name = "Action Name")]
        public string ActionName { get; set; }

    }
}
