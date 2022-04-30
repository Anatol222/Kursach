using ProfileClassLibrary.BusClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Курсовая.MainFrameForms.SityBusPages
{
    public class Directions : StationsPage
    {
        public static Direction firstDirections { get; set; }
        public static Direction secondDirections { get; set; }

        public static List<Direction> GetDirections(string busNumber)
        {
            string drName = GetDirectionFromBD(queryFirstRoute);
            firstDirections = new Direction(drName) { busStations = Direction.GetStations(drName, "First", busNumber) };
            drName = GetDirectionFromBD(querySecondRoute);
            if (drName != null)
            {
                secondDirections = new Direction(drName) { busStations = Direction.GetStations(drName, "Second", busNumber) };
                return new List<Direction>() { firstDirections, secondDirections };
            }
            return new List<Direction>() { firstDirections };
        }
        private static string GetDirectionFromBD(string query)
        {
            DataBase data = new DataBase();
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        return ((string)reader.GetValue(0));
                }

            }
            catch (Exception) { }
            finally{ data.CloseConnection();}
            return null;
        }
    }
}
