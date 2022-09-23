using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Repository.Entites;
using System.Text;

namespace Repository.SeedDatabase
{
    public static class DatabaseSeed
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
        public static void Initializer(IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<TicketingContext>();
                db.Database.EnsureCreated();

                var users = serviceScope.ServiceProvider.GetRequiredService<User>();
                var roles = serviceScope.ServiceProvider.GetRequiredService<Role>();
                var userRoles = serviceScope.ServiceProvider.GetRequiredService<UserRole>();
                var rolePermissions = serviceScope.ServiceProvider.GetRequiredService<RolePermission>();

                if (!db.Users.Any(user => user.UserName == "admin123"))
                {
                    var usr = new User
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
                    db.Users.Add(usr);
                    db.SaveChanges();
                }
                if (!db.Roles.Any(role => role.Name == "SuperAdmin"))
                {
                    var role = new Role
                    {
                        Id = "572a4e6a-917c-4151-9446-fbdc0e2338d4",
                        Name = "SuperAdmin",
                    };
                    db.Roles.Add(role);
                    db.SaveChanges();
                }
                if (!db.RolePermissions.Any(permission => permission.RoleName == "SuperAdmin"))
                {
                    var permissionRoles = new RolePermission();
                    var roleName = "SuperAdmin";
                    var permission = db.Permissions.ToList();
                    var rlePermission = db.RolePermissions.ToList();
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
                    db.RolePermissions.Add(permissionRoles);
                    db.SaveChanges();
                }
            }
        }
    }
}

            
        
    
  

