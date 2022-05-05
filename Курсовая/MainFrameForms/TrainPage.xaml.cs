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

namespace Курсовая.MainFrameForms
{
    /// <summary>
    /// Логика взаимодействия для TrainPage.xaml
    /// </summary>
    public partial class TrainPage : Page
    {
        public TrainPage()
        {
            InitializeComponent();
            DataContext= this;
            Trains = GetTrains();
        }
        public List<Train> Trains { get; set; }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RouteHL_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TrainNameHL_Click(object sender, RoutedEventArgs e)
        {

        }
        private List<Train> GetTrains()
        {
            return new List<Train>() {new Train("Межрегиональные линии экономкласса","603B", "Гомель — Брест-Центральный"
                ,"02:48","Пинск","05:57","Брест-Центральный","3 ч 09 мин") };
        }

        private void trainRoute_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    public class Train
    {
        public Train(string trainType,string trainNumber, string trainRoute, string departureTime, string departureSity, string arrivalTime, string arrivalSity, string durationTime)
        {
            this.trainType = trainType;
            this.trainNumber = trainNumber;
            this.trainRoute = trainRoute;
            this.departureTime = departureTime;
            this.departureSity = departureSity;
            this.arrivalTime = arrivalTime;
            this.arrivalSity = arrivalSity;
            this.durationTime = durationTime;
        }

        public string trainType { get; set; }
        public string trainNumber { get; set; }
        public string trainName { get {return trainNumber+" "+trainRoute;} }
        public string trainRoute { get; set; }
        public string departureTime { get; set; }
        public string departureSity { get; set; }
        public string arrivalTime { get; set; }
        public string arrivalSity { get; set; }
        public string durationTime { get; set; }

    }
}
