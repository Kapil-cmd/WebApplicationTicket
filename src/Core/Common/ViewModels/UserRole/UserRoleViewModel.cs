using System.ComponentModel.DataAnnotations;

namespace Common.ViewModels.UserRole
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public bool IsSelected { get; set; }
        public List<ListRole> Roles { get; set; }

    }
    public class ListRole
    {
        [Key]
        public string Id { get; set; }
        public string RoleName { get; set; }
    }
}
