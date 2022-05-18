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
            string query = default;
            string drName = GetDirectionFromBD(queryFirstRoute);
            if (SityBusPage.BusType == default)
                query = $"SELECT BStop FROM BusStop  WHERE BusId = (SELECT Id FROM Bus WHERE FirstRoute = '{drName}' AND BusName='{busNumber}') AND BusRoute = 'First'; ";
            else
                query = $"SELECT BStop FROM RegionalBusStop  WHERE RegionalBusId = (SELECT Id FROM RegionalBus WHERE FirstRoute = '{drName}' AND BusName='{busNumber}') AND BusRoute = 'First';"; 
            firstDirections = new Direction(drName) { busStations = Direction.GetStations(query) };
            drName = GetDirectionFromBD(querySecondRoute);
            if (drName != null)
            {
                if (SityBusPage.BusType == default)
                    query = $"SELECT BStop FROM BusStop  WHERE BusId = (SELECT Id FROM Bus WHERE SecondRoute = '{drName}' AND BusName='{busNumber}') AND BusRoute = 'Second'; ";
                else
                    query = $"SELECT BStop FROM RegionalBusStop  WHERE RegionalBusId = (SELECT Id FROM RegionalBus WHERE SecondRoute = '{drName}' AND BusName='{busNumber}') AND BusRoute = 'Second';";
                secondDirections = new Direction(drName) { busStations = Direction.GetStations(query) };
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
