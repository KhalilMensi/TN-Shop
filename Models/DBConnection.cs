using System.Data.SqlClient;

namespace AngularAspCore.Models
{
    public static class DBConnection
    {
        static string DbConnnectionString = @"Data Source=DESKTOP-C76LIFI\SQLEXPRESS;Initial Catalog=PersonalProject;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(DbConnnectionString);
        }
    }
}
