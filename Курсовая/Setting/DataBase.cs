using System.Data.SqlClient;

namespace Курсовая
{
    public class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=Zver;Initial Catalog=allData;Integrated Security=True");
        //SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename= |DataDirectory|\allData.mdf;Integrated Security=True");
        //SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\allData.mdf';Integrated Security=True");

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
        public SqlConnection GetConnection()=>
            sqlConnection;
    }
}
