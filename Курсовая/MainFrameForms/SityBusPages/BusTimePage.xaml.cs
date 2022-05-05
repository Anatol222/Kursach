using ProfileClassLibrary.BusClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;

namespace Курсовая.MainFrameForms.SityBusPages
{
    public partial class BusTimePage : Page
    {
        private Frame BusSheduleFrame;
        private Button ByTicket;
        private Button GoToBucket;

        private DataBase data;
        public static InfoAboutBus aboutBus;
        private IWorkWithBusList workWithBusList;

        const string _weekdayTime = "WeekdayTime", _weekendTime = "WeekendTime", _sundayTime = "Sunday", _saturday = "Saturday";
        readonly List<StopTime> _weekdayList, _weekendList, _sundayList, _saturdayList;
        public List<Day> DaysList { get; set; }
        public List<StopTime> StopTimes { get; private set; }
        public string BusStop { get; private set; }
        public string BusName { get; private set; }
        public string Route { get; private set; }
        public string DayOfWeekBus { get; private set; }
        public string PastTimeBus { get; private set; }
        public string City { get; private set; }
        private string BusRoute { get; set; }
        public string NearestTimeBus { get; private set; }
        
        public BusTimePage(Frame frame, Button ByTicket, Button GoToBucket, string bStop, string route, string busName, string busRoute, string city)
        {
            InitializeComponent();

            DaysList = new List<Day>();
            aboutBus = new InfoAboutBus(route, busName, bStop,city,busRoute);
            data = new DataBase();
            workWithBusList = new WorkWithBusList();

            this.ByTicket = ByTicket;
            this.GoToBucket = GoToBucket;
            BusSheduleFrame = frame;
            ByTicket.Visibility = Visibility.Visible;
            GoToBucket.Visibility = Visibility.Visible;
            DataContext = this;

            BusName = busName;
            Route = route;
            BusStop = bStop;
            BusRoute = busRoute;
            City = city;

            _weekdayList = GetTimeBD(_weekdayTime, "Будни");
            _weekendList = GetTimeBD(_weekendTime, "Выходные");
            _saturdayList = GetTimeBD(_saturday, "Суббота");
            _sundayList = GetTimeBD(_sundayTime, "Воскресенье");
            if ((int)DateTime.Now.DayOfWeek <= 5)
                SwitchDay(_weekdayList.ToList(), "Будни");
            else 
            {
                if ((int)DateTime.Now.DayOfWeek == 6 && _saturdayList.Count!=0)
                    SwitchDay(_saturdayList.ToList(), "Суббота");
                else if ((int)DateTime.Now.DayOfWeek ==7 && _sundayList.Count!=0)
                    SwitchDay(_sundayList.ToList(), "Воскресенье");
                else if (_weekendList.Count != 0)
                    SwitchDay(_weekendList.ToList(), "Выходные");
            }
            PastTimeBus = GetPastTime(_weekdayList).ToShortTimeString() + " ( " + Math.Truncate((DateTime.Now - GetPastTime(_weekdayList)).TotalMinutes) + " мин. назад )";
            NearestTimeBus = GetNearestTime(_weekdayList).ToShortTimeString() + " (Через: " + Math.Ceiling((GetNearestTime(_weekdayList) - DateTime.Now).TotalMinutes) + " мин.)";
            InfoBus.Text = $"Расписание автобуса {busName} на остановке {bStop} - {city}";
            if(SityBusPage.BusType != default)
                StationBtn.IsEnabled = false;
        }
        private void SwitchDay(List<StopTime> stopTimes, string day)
        {
            InitializeComponent();
            BusNaumList.ItemsSource = stopTimes;
            StopTimes = stopTimes;
            daysTextBlock.Text = day;
        }
        private void StationBtn_Click(object sender, RoutedEventArgs e)=>
            BusSheduleFrame.NavigationService.Navigate(new BusesOnStationPage(BusSheduleFrame,ByTicket,GoToBucket,BusStop,City));

        private void StopTime_Click(object sender, RoutedEventArgs e)
        {
            DateTime s =Convert.ToDateTime( ((TextBlock)((Button)sender).Content).Text);
            BusSheduleFrame.NavigationService.Navigate(new TimeOfDirection(BusSheduleFrame, ByTicket, GoToBucket,City, s, BusName, daysTextBlock.Text, BusStop,Route,BusRoute));
        }

        private void DaysListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DaysList[((ListBox)sender).SelectedIndex].Name =="Будни")
                SwitchDay(_weekdayList.ToList(), "Будни");
            else 
            {
                if (DaysList[((ListBox)sender).SelectedIndex].Name == "Выходные")
                    SwitchDay(_weekendList.ToList(), "Выходные");
                else if (DaysList[((ListBox)sender).SelectedIndex].Name == "Суббота")
                    SwitchDay(_saturdayList.ToList(), "Суббота");
                else if (DaysList[((ListBox)sender).SelectedIndex].Name == "Воскресенье")
                    SwitchDay(_sundayList.ToList(), "Воскресенье");
            }
        }
        private DateTime GetNearestTime(List<StopTime> timeList)
        {
            foreach (var item in timeList)
                foreach (var time in item.StopTimeList)
                    if (DateTime.Now < time.Time)
                        return time.Time;
            return timeList[0].StopTimeList[0].Time;
        }
        private DateTime GetPastTime(List<StopTime> timeList)
        {
            for (int i = timeList.Count - 2; i >= 0; i--)
                for (int j = timeList[i].StopTimeList.Count() - 1; j >= 0; j--)
                    if (DateTime.Now > timeList[i].StopTimeList[j].Time)
                        return timeList[i].StopTimeList[j].Time;
            return timeList[timeList.Count - 1].StopTimeList[0].Time;
        }
        private List<StopTime> GetTimeBD(string dayOfWeek, string dayOf)
        {
            string query = default;
            if(SityBusPage.BusType == default)
               query = $"SELECT {dayOfWeek} FROM BusStop  WHERE BStop = '{BusStop}' AND BusId = (SELECT Id FROM Bus WHERE {BusRoute}Route = '{Route}' AND BusName = '{BusName}') AND BusRoute = '{BusRoute}'; ";
            else
                query = $"SELECT {dayOfWeek} FROM RegionalBusStop  WHERE BStop = '{BusStop}' AND RegionalBusId = (SELECT Id FROM RegionalBus WHERE {BusRoute}Route = '{Route}' AND BusName = '{BusName}') AND BusRoute = '{BusRoute}';";
            List<Times> listTimes = new List<Times>();
            List<StopTime> timeList = new List<StopTime>() { };
            SqlCommand commad = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = commad.ExecuteReader();
                if (reader.HasRows)
                {
                    string[] getTime = new string[] { };
                    while (reader.Read())
                        getTime = ((string)reader.GetValue(0)).Split(' ');
                    List<string> list = new List<string>();
                    list.AddRange(getTime);
                    var hour = list.GroupBy(x => x.Split(':')[0]);
                    foreach (var item in hour)
                    {
                        foreach (var allTimeInHour in item)
                        {
                            try { listTimes.Add(new Times(Convert.ToDateTime(allTimeInHour)));}
                            catch(Exception) { }
                        }
                        timeList.Add(new StopTime(item.Key, listTimes));
                        listTimes = new List<Times>();
                    }
                }
            }
            catch(Exception) { }
            finally{ data.CloseConnection();}
            if (timeList.Count!=0)
                DaysList.Add(new Day(dayOf));
            return timeList;
        }
        private void TimeList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)=>
            workWithBusList.BucketListBoxSecond(sender, e);
    }
}


