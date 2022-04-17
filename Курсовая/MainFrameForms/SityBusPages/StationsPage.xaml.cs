using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;

namespace Курсовая.MainFrameForms.SityBusPages
{
    public partial class StationsPage : Page
    {
        protected static string querySecondRoute = default;
        protected static string queryFirstRoute = default;
        public StationsPage() {}

        public StationsPage(string busNum, string sity, Border BackBorder)
        {
            InitializeComponent();

            DataContext = this;
            BackBorder.Visibility = Visibility.Visible;

            queryFirstRoute = $"SELECT FirstRoute FROM Bus WHERE PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City = '{sity}') AND BusName = '{busNum}'; ";
            querySecondRoute = $"SELECT SecondRoute FROM Bus WHERE PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City = '{sity}') AND BusName = '{busNum}'; ";
            directions = Directions.GetDirections(busNum);
        }

        public List<Direction> directions { get; set; }
        private void StationsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class Direction
    {
        
        public Direction(string name)=>
            DrName = name;

        public string DrName { get; set; }

        public List<Busstation> _busStations;
        public List<Busstation> busStations { get { return _busStations; } set { _busStations = value; } }
        public static List<Busstation> GetStations(string _drName,string busRoute,string busNumber)
        {
            DataBase data = new DataBase();
            List<Busstation> station = new List<Busstation>();
            string query = $"SELECT BStop FROM BusStop  WHERE BusId = (SELECT Id FROM Bus WHERE {busRoute}Route = '{_drName}' AND BusName='{busNumber}') AND BusRoute = '{busRoute}'; ";
            SqlCommand command = new SqlCommand(query,data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        station.Add(new Busstation() { StName = (string)reader.GetValue(0) });
                }
            }
            catch (Exception) { }
            return station;
        }

    }

    public class Directions:StationsPage
    {

        public static Direction firstDirections { get; set; }
        public static Direction secondDirections { get; set; }

        public static List<Direction> GetDirections(string busNumber)
        {
            string drName = GetDirectionFromBD(queryFirstRoute);
            firstDirections = new Direction(drName) { busStations = Direction.GetStations(drName, "First",busNumber) };
            drName = GetDirectionFromBD(querySecondRoute);
            if (drName!= null)
            {
                secondDirections = new Direction(drName) { busStations = Direction.GetStations(drName,"Second",busNumber) };
                return new List<Direction>() { firstDirections, secondDirections };
            }
            return new List<Direction>() { firstDirections};
        }
        private static string GetDirectionFromBD( string query)
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
            catch{}
            finally
            {
                data.CloseConnection();
            }
            return null;
        }
    }

    public class Busstation
    {
        public string StName { get; set; }
    }
}
