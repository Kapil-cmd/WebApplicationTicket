using System.ComponentModel.DataAnnotations;

namespace Common.ViewModels.Role
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage ="RoleName is required"),MaxLength(200),Display(Name="RoleName")]
        public string Name { get; set; }
    }
    public class EditRole
    {
        public string Id { get; set; }
        [MaxLength(25),Display(Name="RoleName")]
        public string Name { get; set; }
        public List<ListPermission> ListPermission { get; set; }
    }
    public class ListPermission
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
