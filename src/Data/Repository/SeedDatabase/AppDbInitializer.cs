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

                //Add User
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
                        ActivationCode = Guid.Parse("{2c05c682-704f-43a4-864b-742ae359aa30}"),
                        Age = 25,
                        Status = true,
                    };
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                //Add Role
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
                //Add UserRole
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
                //Add RolePermission
                if (!context.RolePermissions.Any())
                {
                    var permissionRoles = new RolePermission();
                    var role = context.Roles.ToList();
                    var permission = context.Permissions.ToList();
                    foreach (var per in permission)
                    {
                        foreach(var rol in role)
                        {
                            permissionRoles.RoleName = rol.Name;
                            permissionRoles.PermissionId = per.PermissionId;
                        }
                        context.RolePermissions.Add(permissionRoles);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
