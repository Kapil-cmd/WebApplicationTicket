using Microsoft.AspNetCore.Builder;
using Repository;
using Repository.Entites;
using Services.CustomFilter;
using System.Reflection;

namespace Services.Middleware
{
    public static class AutoPermissionGenerator //static ommitted for check
    {
        public static void GetPermissions(TicketingContext context)
        {
            try 
            {
                List<RolePermission> permissions = new List<RolePermission>();
                Assembly asm = Assembly.GetEntryAssembly();

                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
                foreach(var assembly in loadedAssemblies)
                {
                    var typePage = assembly.GetTypes().Where(x => x.FullName.Contains("Controllers"));

                    foreach(Type type in typePage)
                    {
                        var methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public).ToList();
                        foreach(var method in methods)
                        {
                            var actionMethod = method.CustomAttributes.Where(x => x.AttributeType.Name == nameof(PermissionFilter)).ToList();
                            foreach (var action in actionMethod)
                            {
                                if (action.ConstructorArguments[0].Value == null)
                                {
                                    continue;
                                }
                                string name = action.ConstructorArguments[0].Value.ToString();

                                List<string> names = name.Split('&').ToList();
                                List<string> groups = names[0].Split('.').ToList();
                                string groupName = "";
                                foreach(var group in groups)
                                {
                                    string parentId = groupName;
                                    if (string.IsNullOrEmpty(parentId))
                                        parentId = null;
                                    groupName += "." + group;
                                    groupName = groupName.TrimStart('.');
                                    if(!permissions.Any(x => x.Name == groupName))
                                    {
                                        permissions.Add(new RolePermission
                                        {
                                            Group = group,
                                            ParentPermissionId = parentId,
                                        });
                                    }
                                }
                                if(!permissions.Any(x => x.Name == name))
                                {
                                    string parentId = permissions.Where(x => x.Name == names[0]).FirstOrDefault()?.Name;
                                    if (string.IsNullOrEmpty(parentId))
                                        parentId = null;
                                    permissions.Add(new RolePermission
                                    {
                                        ParentPermissionId = parentId,
                                        Name = name,
                                        Group = groups.Last(),
                                    });
                                }
                            }
                        }
                    }
                }
                foreach(var model in permissions)
                {
                    if(!context.RolePermissions.Any(x => x.Name == model.Name))
                    {
                        var parentId = context.RolePermissions.Where(x => x.Name == model.ParentPermissionId).FirstOrDefault()?.Name;
                        context.Permissions.Add(new Permission()
                        {
                            Name = model.Name,
                            Group = model.Group,
                            ParentPermissionId = parentId,
                        });
                        context.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
            }
        }
       
    }
    
}
