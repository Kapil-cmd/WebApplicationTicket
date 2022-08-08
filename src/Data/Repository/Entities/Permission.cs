using Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class Permission
    {
        public string PermissionId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public virtual ICollection<RolePermission> Roles { get; set; }

        //public static List<string> GeneratePermissionsList(string module)
        //{
        //    return new List<string>()
        //    {
        //        $"Permissions.{module}.View",
        //        $"Permissions.{module}.Create",
        //        $"Permissions.{module}.Edit",
        //        $"Permissions.{module}.Delete",
        //    };
        //}
        //public static List<string> GenerateAllPermissions()
        //{
        //    var allPermissions = new List<string>();

        //    var modules = Enum.GetValues(typeof(Module));
        //    foreach (var module in Enum.GetValues(typeof(Module)))
        //        allPermissions.AddRange(GeneratePermissionsList(module.ToString()));

        //    return allPermissions;
        //}
    }
}
