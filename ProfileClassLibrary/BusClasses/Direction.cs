using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProfileClassLibrary.BusClasses
{
    public class Direction
    {
        public string DrName { get; set; }

        public List<Busstation> _busStations;
        public List<Busstation> busStations { get { return _busStations; } set { _busStations = value; } }

        public Direction(string name) =>
            DrName = name;

        public static List<Busstation> GetStations(string query)
        {
            DataBase data = new DataBase();
            List<Busstation> station = new List<Busstation>();
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        station.Add(new Busstation() { StName = (string)reader.GetValue(0) });
            }
            catch (Exception) { data.CloseConnection(); }
            return station;
        }
    }
}
