using ProfileClassLibrary.BusClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Курсовая.MainFrameForms.SityBusPages
{
    public partial class TimeOfDirection : Page
    {
        private Frame BusSheduleFrame;
        private Button ByTicket;
        private Button GoToBucket;
        private DataBase data;

        public DateTime nextTime { get; private set; }
        public string Route { get; private set; }
        public string BusRoute { get; private set; }
        public string BusStop { get; private set; }
        public string BusName { get; private set; }
        public string DayOfWeekBus { get; private set; }
        public string StringDayOfWeekBus { get; private set; }
        public string StringTime { get; private set; }
        public string City { get; private set; }
        public List<AllInfoAboutLocationBus> AllInfoAbouts { get; set; }

        public TimeOfDirection(Frame frame, Button ByTicket, Button GoToBucket, string city, DateTime time,string busName,string dayOfWeekBus,string busStopName,string route,string busRoute)
        {
            InitializeComponent();
            DataContext = this;
            nextTime = time;
            BusName = busName;
            BusStop = busStopName;
            Route = route;
            BusRoute = busRoute;
            nextTime = time;
            City = city;
            StringTime = time.ToShortTimeString();
            StringDayOfWeekBus = dayOfWeekBus;
            if (dayOfWeekBus == "Будни")
                DayOfWeekBus = "WeekdayTime";
            else if (dayOfWeekBus == "Выходные")
                DayOfWeekBus = "WeekendTime";
            else if (dayOfWeekBus == "Суббота")
                DayOfWeekBus = "Saturday";
            else if (dayOfWeekBus == "Воскресенье")
                DayOfWeekBus = "Sunday";
            string query = default;
            if (SityBusPage.BusType == default)
                query = $"SELECT BStop FROM BusStop  WHERE BusId = (SELECT Id FROM Bus WHERE {BusRoute}Route = '{Route}' AND BusName = '{BusName}') AND BusRoute = '{BusRoute}'; ";
            else
                query = $"SELECT BStop FROM RegionalBusStop  WHERE RegionalBusId = (SELECT Id FROM RegionalBus WHERE {BusRoute}Route = '{Route}' AND BusName = '{BusName}') AND BusRoute = '{BusRoute}'; ";
            data = new DataBase();
            this.ByTicket = ByTicket;
            this.GoToBucket = GoToBucket;
            BusSheduleFrame = frame;

            AllInfoAbouts = GetInfoBus(GetBusStop(query), busStopName);
        }
        private void StationBtn_Click(object sender, RoutedEventArgs e)=>
            BusSheduleFrame.NavigationService.Navigate(new BusesOnStationPage(BusSheduleFrame, ByTicket, GoToBucket, BusStop, City));

        private DateTime GetNearestTime(List<StopDateTime> timeList)
        {
            foreach (var item in timeList)
                foreach (var time in item.StopTimeList)
                    if (nextTime < time)
                    { nextTime = time; return time; }
            return timeList[0].StopTimeList[0];
        }
        private DateTime GetPastTime(List<StopDateTime> timeList)
        {
            for (int i = timeList.Count - 2; i >= 0; i--)
                for (int j = timeList[i].StopTimeList.Count() - 1; j >= 0; j--)
                    if (nextTime > timeList[i].StopTimeList[j])
                    { nextTime = timeList[i].StopTimeList[j]; return timeList[i].StopTimeList[j]; }
            return timeList[timeList.Count - 1].StopTimeList[0];
        }
        private List<AllBusStop> GetBusStop(string query)
        {
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            List<AllBusStop> busStopList = new List<AllBusStop>();
            data.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        busStopList.Add(new AllBusStop((string)reader.GetValue(0)));
                
                reader.Close();
            }
            catch (Exception){ }
            finally { data.CloseConnection();}
            return busStopList;
        }
        private List<AllInfoAboutLocationBus> FillInInfoBus(List<AllBusStop> before, List<AllBusStop> after)
        {
            DateTime beforeTime = nextTime;
            List<AllInfoAboutLocationBus> busList = new List<AllInfoAboutLocationBus>();
            List<AllInfoAboutLocationBus> finishBusList = new List<AllInfoAboutLocationBus>();
            for (int i = before.Count() - 1; i >= 0; i--)
                busList.Add(GetDataFromBD(before[i], true));
            for (int i = busList.Count() - 1; i >= 0; i--)
                finishBusList.Add(busList[i]);
            nextTime = beforeTime;
            for (int i = 0; i < after.Count; i++)
            {
                if (i == 0) { finishBusList.Add(new AllInfoAboutLocationBus() { BusStop = after[i], Time = nextTime }); }
                else
                    finishBusList.Add(GetDataFromBD(after[i], false));
            }
            return finishBusList;
        }
        private AllInfoAboutLocationBus GetDataFromBD(AllBusStop allBusStops, bool beforeTrue)
        {
            AllInfoAboutLocationBus busList = new AllInfoAboutLocationBus();
            List<DateTime> stopTimeList = new List<DateTime>();
            List<StopDateTime> timeList = new List<StopDateTime>() { };
            string query = default;
            if (SityBusPage.BusType == default)
                query = $"SELECT {DayOfWeekBus} FROM BusStop  WHERE BStop = '{allBusStops.BusStopName}' AND BusId = (SELECT Id FROM Bus WHERE {BusRoute}Route = '{Route}' AND BusName = '{BusName}') AND BusRoute = '{BusRoute}'; ";
            else
                query = $"SELECT {DayOfWeekBus} FROM RegionalBusStop  WHERE BStop = '{allBusStops.BusStopName}' AND RegionalBusId = (SELECT Id FROM RegionalBus WHERE {BusRoute}Route = '{Route}' AND BusName = '{BusName}') AND BusRoute = '{BusRoute}';";
            SqlCommand commad = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = commad.ExecuteReader();
                if (reader.HasRows) { }
                else
                {
                    if (SityBusPage.BusType == default)
                        commad = new SqlCommand($"SELECT WeekendTime FROM BusStop  WHERE BStop = '{allBusStops.BusStopName}' AND BusId = (SELECT Id FROM Bus WHERE {BusRoute}Route = '{Route}' AND BusName = '{BusName}') AND BusRoute = '{BusRoute}'; ", data.GetConnection());
                    else
                        commad = new SqlCommand($"SELECT WeekendTime FROM RegionalBusStop  WHERE BStop = '{allBusStops.BusStopName}' AND RegionalBusId = (SELECT Id FROM RegionalBus WHERE {BusRoute}Route = '{Route}' AND BusName = '{BusName}') AND BusRoute = '{BusRoute}'; ", data.GetConnection());
                    reader = commad.ExecuteReader();
                }
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
                            stopTimeList.Add(Convert.ToDateTime(allTimeInHour));
                        timeList.Add(new StopDateTime(item.Key, stopTimeList));
                        stopTimeList = new List<DateTime>();
                    }
                    if (beforeTrue)
                        busList = new AllInfoAboutLocationBus() { BusStop = allBusStops, Time = GetPastTime(timeList) };
                    else
                        busList = new AllInfoAboutLocationBus() { BusStop = allBusStops, Time = GetNearestTime(timeList) };
                }
            }
            catch(Exception) { }
            finally { data.CloseConnection(); }
            return busList;
        }
        private List<AllInfoAboutLocationBus> GetInfoBus(List<AllBusStop> busStopList, string busStopName)
        {
            List<AllBusStop> before = new List<AllBusStop>();
            List<AllBusStop> after = new List<AllBusStop>();
            bool afterBusStop = false;
            foreach (var busStop in busStopList)
            {
                if (busStop.BusStopName == busStopName || afterBusStop)
                {
                    afterBusStop = true;
                    after.Add(busStop);
                }
                else
                    before.Add(busStop);
            }
            return FillInInfoBus(before, after);
        }
    }
}
