using Microsoft.Data.SqlClient;

namespace Web.StaticModel
{
    public static class PermissionChecker
    {
        public static bool HasPermission(string username, string permissionValue)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "Server=DESKTOP-BQOVRGO\\SQLEXPRESS;Database=N-TierTicket;Trusted_Connection=True;MultipleActiveResultSets=true";
                conn.Open();

                string commandtext = "select * from Users";
                SqlCommand cmd = new SqlCommand(commandtext, conn);
                
            }
        }
           
    }
}

