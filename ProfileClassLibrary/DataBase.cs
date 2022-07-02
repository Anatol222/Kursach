using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileClassLibrary
{
    public class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["allData"].ConnectionString);
        //SqlConnection sqlConnection = new SqlConnection(@"Data Source=Zver;Initial Catalog=allData;Integrated Security=True");
        public void OpenConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
                sqlConnection.Open();
        }
        public void CloseConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
                sqlConnection.Close();
        }
        public SqlConnection GetConnection() =>
            sqlConnection;
    }
}
