using Microsoft.Data.SqlClient;

namespace MultiShop.Models
{
    public class Connection
    {
        public static SqlConnection ServerConnect
        {
            get
            {
                SqlConnection sqlConnection = new SqlConnection("Server=DESKTOP-TCALSD6;Database=MultiShop;Trusted_Connection=True;TrustServerCertificate=True;");
                return sqlConnection;
            }
        }
    }
}
