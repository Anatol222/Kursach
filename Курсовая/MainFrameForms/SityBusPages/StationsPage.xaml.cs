using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Windows.Media;
using System.Windows.Input;

namespace Курсовая.MainFrameForms.SityBusPages
{
    public partial class StationsPage : Page
    {
        protected static string querySecondRoute = default;
        protected static string queryFirstRoute = default;
        Button ByTicket;
        Button GoToBucket;
        Frame BusSheduleFrame;
        public StationsPage() {}

        public StationsPage(string busNum, string sity, Frame BusSheduleFrame,Button ByTicket,Button GoToBucket, Border BackBorder)
        {
            InitializeComponent();

            DataContext = this;
            this.ByTicket = ByTicket;
            this.GoToBucket = GoToBucket;
            this.BusSheduleFrame = BusSheduleFrame;
            BackBorder.Visibility = Visibility.Visible;

            queryFirstRoute = $"SELECT FirstRoute FROM Bus WHERE PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City = '{sity}') AND BusName = '{busNum}'; ";
            querySecondRoute = $"SELECT SecondRoute FROM Bus WHERE PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City = '{sity}') AND BusName = '{busNum}'; ";
            directions = Directions.GetDirections(busNum);
        }

        public List<Direction> directions { get; set; } = new List<Direction>() { new Direction("123") { busStations = new List<Busstation>() { new Busstation() {StName = "21#4124" } } },new Direction("123") { busStations = new List<Busstation>() { new Busstation() {StName = "21#4124" } } } };

        private void BusStationNav_Click(object sender, RoutedEventArgs e)
        {
            BusSheduleFrame.NavigationService.Navigate(new BusTimePage(BusSheduleFrame, ByTicket, GoToBucket));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ByTicket.Visibility = Visibility.Hidden;
            GoToBucket.Visibility = Visibility.Hidden;
        }
        private void StationsListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (null == listBox)
            {
                return;
            }

            var point = e.GetPosition((UIElement)sender);

            VisualTreeHelper.HitTest(listBox, null, (hitTestResult) =>
            {
                var uiElement = hitTestResult.VisualHit as UIElement;

                while (null != uiElement)
                {
                    var listBoxItem = uiElement as ListBoxItem;
                    if (null != listBoxItem)
                    {
                        listBoxItem.IsSelected = true;
                        return HitTestResultBehavior.Stop;
                    }

                    uiElement = VisualTreeHelper.GetParent(uiElement) as UIElement;
                }

                return HitTestResultBehavior.Continue;
            }, new PointHitTestParameters(point));
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
