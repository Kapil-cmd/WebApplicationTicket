using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Repository.Entites;
using System.Text;

namespace Repository.SeedDatabase
{
    public class AppDbInitializer
    {
        public static class HashPassword
        {
            public static string Hash(string value)
            {
                return Convert.ToBase64String(
                    System.Security.Cryptography.SHA256.Create()
                    .ComputeHash(Encoding.UTF8.GetBytes(value))
                    );
            }
        }
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<TicketingContext>();

                context.Database.EnsureCreated();

                //User
                if (!context.Users.Any())
                {
                    var user = new User
                    {
                        Id = "b74ddd14-6340-4840-95c2-db12554843e5",
                        UserName = "Superadmin123",
                        FirstName = "Super",
                        LastName = "Admin",
                        Address = "Admin",
                        Email = "Superadmin123@gmail.com",
                        PhoneNumber = 9812320579,
                        IsEmailVerified = true,
                        Password = HashPassword.Hash("admin123"),
                        DateOfBirth = DateTime.Parse("1997-02-12"),
                    };
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                //Role
                if (!context.Roles.Any())
                {
                    var role = new Role
                    {
                        Id = "572a4e6a-917c-4151-9446-fbdc0e2338d4",
                        Name = "SuperAdmin",
                    };
                    context.Roles.Add(role);
                    context.SaveChanges();
                }
                //USerRole
                if (!context.UserRoles.Any())
                {
                    var userRoles = new UserRole
                    {
                        UserName = "Superadmin123",
                        RoleName = "SuperAdmin",
                    };
                    context.UserRoles.Add(userRoles);
                    context.SaveChanges();
                }
                //RolePermission
                if (!context.RolePermissions.Any())
                {
                    var permissionRoles = new RolePermission();
                    var roleName = "SuperAdmin";
                    var permission = context.Permissions.ToList();
                    var rlePermission = context.RolePermissions.ToList();
                    foreach (var per in permission)
                    {
                        foreach (var pm in rlePermission)
                        {
                            pm.PermissionId = per.PermissionId;
                            pm.RoleName = roleName;

                            permissionRoles.PermissionId = pm.PermissionId;
                            permissionRoles.RoleName = pm.RoleName;
                        }
                    }
                    context.RolePermissions.Add(permissionRoles);
                    context.SaveChanges();
                }
            }
        }
    }
}
