using Microsoft.Data.SqlClient;

namespace Web.StaticModel
{
    public static class PermissionChecker
    {
        public static bool HasPermission(string username, string permissionValue)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Server=DESKTOP-BQOVRGO\\SQLEXPRESS;Database=Ticket;Trusted_Connection=True;MultipleActiveResultSets=true"))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select Case when ISNULL(PermissionId, '') = '' then 0 Else 1 End HasPermission from RolePermissions join Roles on RolePermissions.RoleName = Roles.Name join UserRoles on Roles.Name = UserRoles.RoleName join Users on UserRoles.UserName = USERS.UserName where Users.UserName = @username and RolePermissions.PermissionId =@permission;";
                        cmd.CommandType = System.Data.CommandType.Text;

                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("permission", permissionValue);

                        con.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    bool hasPermission = Convert.ToBoolean(reader["HasPermission"]);
                                    return hasPermission;
                                }
                            }
                        }
                        con.Close();
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}



