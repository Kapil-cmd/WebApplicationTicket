using Repository;
using Repository.Entites;
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

                }
            }catch(Exception ex)
            {

            }
         }

    }
}
