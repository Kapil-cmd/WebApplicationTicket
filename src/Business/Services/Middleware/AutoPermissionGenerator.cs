using Repository;
using Repository.Entites;
using Services.CustomFilter;
using System.Reflection;

namespace Services.Middleware
{
    public static class AutoPermissionGenerator
    {
        public static void GetPermission(TicketingContext context)
        {
            try
            {
                List<RolePermission> permissions = new List<RolePermission>();
                Assembly asm = Assembly.GetEntryAssembly();

                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
                foreach (var assembly in loadedAssemblies)
                {
                    var typePage = assembly.GetTypes().Where(x => x.FullName.Contains("Controllers"));

                    foreach(Type type in typePage)
                    {
                        var methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public).ToList();

                        foreach(var method in methods)
                        {
                            var actionMethod = method.CustomAttributes.Where(x => x.AttributeType.Name == nameof(PermissionFilter)).ToList();
                            foreach(var action in actionMethod)
                            {
                                if (action.ConstructorArguments[0].Value == null)
                                {
                                    continue;
                                }

                                string slug = action.ConstructorArguments[0].Value.ToString();

                                List<string> slugs = slug.Split('&').ToList();
                                List<string> groups = slugs[0].Split('.').ToList();
                                string groupSlug = "";
                                foreach(var group in groups)
                                {
                                    string parentId = groupSlug;
                                    if (string.IsNullOrEmpty(parentId))
                                        parentId = null;
                                    groupSlug += "." + group;
                                    groupSlug = groupSlug.TrimStart('.');
                                    if(!permissions.Any(x => x.Slug == groupSlug))
                                    {
                                        permissions.Add(new RolePermission
                                        {
                                            Group = group,
                                            Slug = groupSlug,
                                            ParentPermissionId = parentId,
                                        });
                                    }
                                }
                                if(!permissions.Any(x => x.Slug == slug))
                                {
                                    string parentId = permissions.Where(x => x.Slug == slugs[0]).FirstOrDefault()?.Slug;
                                    if (string.IsNullOrEmpty(parentId))
                                        parentId = null;
                                    permissions.Add(new RolePermission
                                    {
                                        ParentPermissionId = parentId,
                                        Slug = slug,
                                        Group = groups.Last(),
                                    });
                                }
                            }
                        }
                    }
                }
                //foreach(var model in permissions)
                //{
                //    if(!context.RolePermissions.Any(x => x.Slug == model.Slug))
                //    {
                //        var parentId = context.RolePermissions.Where(x => x.Slug == model.ParentPermissionId).FirstOrDefault()?.Slug;
                //        context.RolePermissions.Add(new Permission()
                //        {
                //            Slug = model.Slug,
                //            Group = model.Group,
                //            ParentPermissionId = parentId,
                //        });
                //        context.SaveChanges();
                //    }
                //}
            }
            catch(Exception ex)
            {
            }
         }

    }
}
