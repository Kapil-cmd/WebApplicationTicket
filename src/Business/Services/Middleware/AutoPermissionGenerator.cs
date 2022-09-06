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
                List<Permission> permissions = new List<Permission>();
                Assembly asm = Assembly.GetEntryAssembly();

                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
                foreach (var assembly in loadedAssemblies)
                {
                    var typePage = assembly.GetTypes().Where(x => x.FullName.Contains("Controllers"));

                    foreach (Type type in typePage)
                    {
                        var methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public).ToList();
                        foreach (var method in methods)
                        {
                            var actionMethod = method.CustomAttributes.Where(x => x.AttributeType.Name == nameof(PermissionFilter)).ToList();
                            foreach (var action in actionMethod)
                            {
                                if (action.ConstructorArguments[0].Value == null)
                                {
                                    continue;
                                }

                                string slug = action.ConstructorArguments[0].Value.ToString();


                                List<string> slugs = slug.Split('&').ToList();

                                string parentId = "";

                                foreach (var item in slugs)
                                {
                                    if (!context.Permissions.Any(x => x.Slug == item))
                                    {
                                        context.Permissions.Add(new Permission()
                                        {
                                            PermissionId = item,
                                            Slug = item,
                                            ParentId = parentId,
                                            MenuName = item.Replace("_", " "),
                                        });
                                        context.SaveChanges();
                                    }
                                    parentId = item;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
