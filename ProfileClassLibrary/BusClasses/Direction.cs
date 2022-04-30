using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProfileClassLibrary.BusClasses
{
    public class Direction
    {
        public Direction(string name) =>
            DrName = name;
        public string DrName { get; set; }

        public List<Busstation> _busStations;
        public List<Busstation> busStations { get { return _busStations; } set { _busStations = value; } }
        public static List<Busstation> GetStations(string _drName, string busRoute, string busNumber)
        {
            DataBase data = new DataBase();
            List<Busstation> station = new List<Busstation>();
            string query = $"SELECT BStop FROM BusStop  WHERE BusId = (SELECT Id FROM Bus WHERE {busRoute}Route = '{_drName}' AND BusName='{busNumber}') AND BusRoute = '{busRoute}'; ";
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
