using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
