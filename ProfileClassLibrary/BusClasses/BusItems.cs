using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProfileClassLibrary.BusClasses
{
    public class BusItems
    {
        public static List<Bus> GetBuses(string city)
        {
            SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-HOHELQO;Initial Catalog=allData;Integrated Security=True");
            List<Bus> busList = new List<Bus>();
            SqlCommand command = new SqlCommand($"SELECT BusName FROM Bus WHERE PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City='{city}');", sqlConnection);
            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                    busList.Add(new Bus() { Number = (string)reader.GetValue(0) });
            reader.Close();
            sqlConnection.Close();
            return busList;
        }
    }
}
