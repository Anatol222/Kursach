using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.IO;

namespace Курсовая
{
    public class DataBase
    {
        //SqlConnection sqlConnection = new SqlConnection(@"Data Source=Zver;Initial Catalog=allData;Integrated Security=True");
        //SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename= |DataDirectory|\allData.mdf;Integrated Security=True");
        //SqlConnection sqlConnection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLSERVER;AttachDbFilename=|DataDirectory|\allData.mdf;Integrated Security = True");

        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["allData"].ConnectionString);

        public void OpenConnection()
        {
            //var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
                sqlConnection.Open();
        }
        public void CloseConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
                sqlConnection.Close();
        }
        public SqlConnection GetConnection()=>
            sqlConnection;
    }
}
