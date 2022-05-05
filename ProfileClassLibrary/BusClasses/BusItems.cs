using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProfileClassLibrary.BusClasses
{
    public class BusItems
    {
        public static List<Bus> GetBuses(string query)
        {
            DataBase data = new DataBase();
            List<Bus> busList = new List<Bus>();
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        busList.Add(new Bus() { Number = (string)reader.GetValue(0) });
                reader.Close();
            }
            catch (Exception) { }
            finally { data.CloseConnection(); }
            return busList;
        }
    }
}
