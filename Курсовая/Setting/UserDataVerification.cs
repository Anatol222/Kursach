using Курсовая.ProgrammInterface;
using System.Data.SqlClient;
using System.Data;

namespace Курсовая.Setting
{
    internal class UserDataVerification : IDataBaseUserDataVerification
    {
        private SaveNewUserData saveNewUserData;
        private DataBase dataBase;

        public UserDataVerification()=>
            dataBase = new DataBase();

        public void Display(string conten, string buttonText)
        {
            if (saveNewUserData != null)
                saveNewUserData.Close();
            saveNewUserData = new SaveNewUserData(conten,buttonText);
            saveNewUserData.ShowDialog();
        }


        public bool Verification(string query)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            try
            {
                dataBase.OpenConnection();
                SqlCommand sqlCommand = new SqlCommand(query, dataBase.GetConnection());
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
            }
            catch (System.Exception) { return false;}
            finally {dataBase.CloseConnection();}
            return dataTable.Rows.Count > 0;
        }
    }
}
