using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Курсовая.MainFrameForms.SityBusPages
{
    /// <summary>
    /// Логика взаимодействия для BusTimePage.xaml
    /// </summary>
    public partial class BusTimePage : Page
    {
        public BusTimePage()
        {
            InitializeComponent();
            DataContext = this;
            StopTimes = GetTimeBD();
        }
        //public List<Day> DaysList { get; set; } = new List<Day>() {
        //    new Day() { _name = "Будни",
        //                _dateTimes = new List<DateTime>() {
        //                    new DateTime(1,1,1,12,30,00)
        //                }
        //    },new Day() { _name = "Выходные",
        //                _dateTimes = new List<DateTime>() {
        //                    new DateTime(1,1,1,12,30,0)
        //                }
        //    }
        //};
        public List<StopTime> StopTimes { get; set; }
        public string Stroka { get; set; } = "привет";
        public List<StopTime> GetTimeBD(/*string query*/)
        {
            List<DateTime> stopTimeList = new List<DateTime>();
            List<StopTime> timeList = new List<StopTime>() { };
            //SqlCommand commad = new SqlCommand(query, data.GetConnection());
            //data.OpenConnection();
            //try
            //{
            //    //SqlDataReader reader = commad.ExecuteReader();
            //    if (reader.HasRows)
            //    {
            //        string[] getTime = new string[] { };
            //        while (reader.Read())
            //            getTime = ((string)reader.GetValue(0)).Split(' ');
            //        List<string> list = new List<string>();
            //        list.AddRange(getTime);
            //        var hour = list.GroupBy(x => x.Split(':')[0]);
            //        foreach (var item in hour)
            //        {
            //            foreach (var allTimeInHour in item)
            //                stopTimeList.Add(Convert.ToDateTime(allTimeInHour));
            //            timeList.Add(new StopTime(item.Key, stopTimeList));
            //            stopTimeList = new List<DateTime>();
            //        }

            //    }
            //}
            //catch { }
            //finally
            //{
            //    data.CloseConnection();
            //}
            return new List<StopTime>()
            {
                new StopTime("6", new List<Times>(){new Times() { Time = new DateTime(1,1,1,7,30,0)},new Times() { Time = new DateTime(1,1,1,7,30,0)} })
                {
                },new StopTime("7", new List<Times>(){new Times() { Time = new DateTime(1,1,1,7,30,0)},new Times() { Time = new DateTime(1,1,1,7,30,0)}})
                {
                },new StopTime("7", new List<Times>(){new Times() { Time = new DateTime(1,1,1,7,30,0)},new Times() { Time = new DateTime(1,1,1,7,30,0)}})
                {
                },new StopTime("7", new List<Times>(){new Times() { Time = new DateTime(1,1,1,7,30,0)},new Times() { Time = new DateTime(1,1,1,7,30,0)}})
                {
                },new StopTime("7", new List<Times>(){new Times() { Time = new DateTime(1,1,1,7,30,0)},new Times() { Time = new DateTime(1,1,1,7,30,0)}})
                {
                },new StopTime("7", new List<Times>(){new Times() { Time = new DateTime(1,1,1,7,30,0)},new Times() { Time = new DateTime(1,1,1,7,30,0)}})
                {
                },new StopTime("7", new List<Times>(){new Times() { Time = new DateTime(1,1,1,7,30,0)},new Times() { Time = new DateTime(1,1,1,7,30,0)}})
                {
                },new StopTime("7", new List<Times>(){new Times() { Time = new DateTime(1,1,1,7,30,0)},new Times() { Time = new DateTime(1,1,1,7,30,0)}})
                {
                },new StopTime("7", new List<Times>(){new Times() { Time = new DateTime(1,1,1,7,30,0)},new Times() { Time = new DateTime(1,1,1,7,30,0)}})
                {
                }
            };
        }

    }
    public class StopTime
    {
        public StopTime(string indexTime, List<Times> stopTimeList)
        {
            _stopTimeList = stopTimeList;
            _indexTime = indexTime;
        }
        private string _indexTime;
        public string IndexTime { get => _indexTime; }

        private List<Times> _stopTimeList;
        public List<Times> StopTimeList { get => _stopTimeList; }

    }
    public class Times{
        public DateTime Time { get; set; }
    }
    public class Day
    {
        public string _name;
        public string Name { get { return _name; } }
        public List<DateTime> _dateTimes;
        public List<DateTime> dateTimes { get { return _dateTimes; } }
    }
    
}


