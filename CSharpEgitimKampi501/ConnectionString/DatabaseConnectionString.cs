using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpEgitimKampi501.ConnectionString
{
    public static class DatabaseConnectionString
    {
        public static void SqlDatabaseConnectionString()
        {
            SqlConnection connection = new SqlConnection("Server=.;Database=EgitimKampi501Db;Trusted_Connection=True;");
        }
        
    }
}
