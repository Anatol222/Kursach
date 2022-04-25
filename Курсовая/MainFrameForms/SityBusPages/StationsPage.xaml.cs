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
        private DataBase dataBase;
        public StationsPage() {}

        public StationsPage(string busNum, string city, Frame BusSheduleFrame,Button ByTicket,Button GoToBucket, Border BackBorder)
        {
            InitializeComponent();
            dataBase = new DataBase();
            DataContext = this;
            this.ByTicket = ByTicket;
            this.GoToBucket = GoToBucket;
            this.BusSheduleFrame = BusSheduleFrame;
            BackBorder.Visibility = Visibility.Visible;

            queryFirstRoute = $"SELECT FirstRoute FROM Bus WHERE PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City = '{city}') AND BusName = '{busNum}'; ";
            querySecondRoute = $"SELECT SecondRoute FROM Bus WHERE PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City = '{city}') AND BusName = '{busNum}'; ";
            directions = Directions.GetDirections(busNum);
            _busName = busNum;
            _city = city;
        }

        public List<Direction> directions { get; set; }
        public Direction direction;
        private string _clickedStation;
        public string ClickedStationName { get { return _clickedStation; } set { _clickedStation = value; } }
        private string _clickedDr;
        public string ClickedDrName { get { return _clickedDr; } set { _clickedDr= value; } }
        private readonly string _busName;
        private readonly string _city;
        private void BusStationNav_Click(object sender, RoutedEventArgs e)=>
            BusSheduleFrame.NavigationService.Navigate(new BusTimePage(BusSheduleFrame, ByTicket, GoToBucket,ClickedStationName,ClickedDrName,_busName,BusRoute(),_city));
        
        private string BusRoute()
        {
            string busRoute = default;
            string query = $"SELECT FirstRoute FROM Bus WHERE BusName = '{_busName}' AND PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City ='{_city}');";
            SqlCommand command = new SqlCommand(query,dataBase.GetConnection());
            dataBase.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        busRoute = (string)reader.GetValue(0);
                reader.Close();
            }
            catch(Exception) { }
            finally { dataBase.CloseConnection(); }
            if (ClickedDrName==busRoute)
                return "First";
            return "Second";
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
                return;

            var point = e.GetPosition((UIElement)sender);

            VisualTreeHelper.HitTest(listBox, null, (hitTestResult) =>
            {
                var uiElement = hitTestResult.VisualHit as UIElement;
                ListBoxItem firstLisBoxItem = null;
                ListBoxItem SecondtLisBoxItem = null;

                while (null != uiElement)
                {
                    if (uiElement == uiElement as ListBoxItem)
                    {
                        if (firstLisBoxItem == null)
                            firstLisBoxItem = uiElement as ListBoxItem;
                        else
                            SecondtLisBoxItem = uiElement as ListBoxItem;
                    }
                    if (firstLisBoxItem != null && SecondtLisBoxItem != null)
                    {
                        SecondtLisBoxItem.IsSelected = true;
                        firstLisBoxItem.IsSelected = true;
                        return HitTestResultBehavior.Stop;
                    }
                    
                    uiElement = VisualTreeHelper.GetParent(uiElement) as UIElement;
                }

                return HitTestResultBehavior.Continue;
            }, new PointHitTestParameters(point));
        }

        private void StationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)=>
            ClickedStationName = direction._busStations[((ListBox)sender).SelectedIndex].StName;

        private void StationsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            direction = directions[((ListBox)sender).SelectedIndex];
            ClickedDrName = direction.DrName;
        }
    }

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
            catch(Exception) {}
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
