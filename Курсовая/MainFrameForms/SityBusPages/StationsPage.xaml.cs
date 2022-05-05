using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Windows.Input;
using ProfileClassLibrary.BusClasses;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;

namespace Курсовая.MainFrameForms.SityBusPages
{
    public partial class StationsPage : Page
    {
        Button ByTicket;
        Button GoToBucket;
        Frame BusSheduleFrame;
        private DataBase dataBase;
        private IWorkWithBusList workWithBusList;

        public List<Direction> directions { get; set; }
        public Direction direction;
        private string _clickedStation;
        public string ClickedStationName { get { return _clickedStation; } set { _clickedStation = value; } }
        private string _clickedDr;
        public string ClickedDrName { get { return _clickedDr; } set { _clickedDr = value; } }
        private readonly string _busName;
        private readonly string _city;
        protected static string querySecondRoute = default;
        protected static string queryFirstRoute = default;
        public StationsPage() {}

        public StationsPage(string busNum, string city, Frame BusSheduleFrame,Button ByTicket,Button GoToBucket, Border BackBorder)
        {
            InitializeComponent();
            
            dataBase = new DataBase();
            workWithBusList = new WorkWithBusList();

            DataContext = this;
            this.ByTicket = ByTicket;
            this.GoToBucket = GoToBucket;
            this.BusSheduleFrame = BusSheduleFrame;
            BackBorder.Visibility = Visibility.Visible;

            if (SityBusPage.BusType == default)
            {
                queryFirstRoute = $"SELECT FirstRoute FROM Bus WHERE PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City = '{city}') AND BusName = '{busNum}'; ";
                querySecondRoute = $"SELECT SecondRoute FROM Bus WHERE PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City = '{city}') AND BusName = '{busNum}'; ";
            }
            else
            {
                queryFirstRoute = $"SELECT FirstRoute FROM RegionalBus WHERE RegionalBusDistrictId = (SELECT Id FROM RegionalBusDistrict WHERE District = '{city}') AND BusName = '{busNum}'; ";
                querySecondRoute = $"SELECT SecondRoute FROM RegionalBus WHERE RegionalBusDistrictId = (SELECT Id FROM RegionalBusDistrict WHERE District = '{city}') AND BusName = '{busNum}'; ";
            }
            directions = Directions.GetDirections(busNum);
            _busName = busNum;
            _city = city;
        }
        private void BusStationNav_Click(object sender, RoutedEventArgs e)=>
            BusSheduleFrame.NavigationService.Navigate(new BusTimePage(BusSheduleFrame, ByTicket, GoToBucket,ClickedStationName,ClickedDrName,_busName,BusRoute(),_city));
        
        private string BusRoute()
        {
            string busRoute = default; string query = default;
            if (SityBusPage.BusType == default)
                query = $"SELECT FirstRoute FROM Bus WHERE BusName = '{_busName}' AND PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City ='{_city}');";
            else
                query = $"SELECT FirstRoute FROM RegionalBus WHERE BusName = '{_busName}' AND RegionalBusDistrictId = (SELECT Id FROM RegionalBusDistrict WHERE District ='{_city}');";
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
        private void StationsListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)=>
            workWithBusList.BucketListBoxSecond(sender,e);

        private void StationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)=>
            ClickedStationName = direction._busStations[((ListBox)sender).SelectedIndex].StName;

        private void StationsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            direction = directions[((ListBox)sender).SelectedIndex];
            ClickedDrName = direction.DrName;
        }
    }
}
