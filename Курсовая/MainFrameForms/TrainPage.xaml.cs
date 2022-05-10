using MaterialDesignThemes.Wpf;
using ProfileClassLibrary.TrainClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Курсовая.MainFrameForms.SityBusPages;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;

namespace Курсовая.MainFrameForms
{
    public partial class TrainPage : Page
    {
        private DataBase data = new DataBase();
        private IDataProcessing dataProcessing;

        private INavigation navigation;
        private IDataBaseUserDataVerification userDataVerification;

        private delegate void InvoceMessageBox(string messange);
        private event InvoceMessageBox Notification;

        private delegate void InvoceWarningBox(string content, string buttonText);
        private event InvoceWarningBox Warning;

        private delegate void InvoceCountTicketsBox(string buttonContent);
        private event InvoceCountTicketsBox TicketEvent;

        private CountOfBusTickets countOfBusTickets;

        public TrainPage()
        {
            InitializeComponent();
            DataContext= this;

            navigation = new ProgrammNavigation();
            userDataVerification = new UserDataVerification();
            Notification = navigation.Display;
            Warning = userDataVerification.Display;
            TicketEvent = Display;

            dataProcessing = new DataProcessing();
            Trains = StartTrain();
            DepartureTrain.DisplayDateStart = DateTime.Now;
            DepartureTrain.Text = DateTime.Now.ToShortDateString();
            ReserveListTrains = Trains.ToList();
        }
        public void Display(string buttonContent)
        {
            if (countOfBusTickets != null)
                countOfBusTickets.Close();
            countOfBusTickets = new CountOfBusTickets(buttonContent);
            countOfBusTickets.ShowDialog();
        }
        public List<Train> Trains { get; set; }
        private List<Train> ReserveListTrains { get; set; }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RouteHL_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TrainNameHL_Click(object sender, RoutedEventArgs e)
        {

        }
        private void trainRoute_Click(object sender, RoutedEventArgs e)
        {

        }
        public List<TrainStation> GetStation(string query, string station)
        {
            List<TrainStation> trainStations = new List<TrainStation>();
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        try
                        {
                            trainStations.Add(new TrainStation() { Id = Convert.ToInt32(reader.GetValue(0)), TrainId = Convert.ToInt32(reader.GetValue(1)), DTime = Convert.ToDateTime(Convert.ToString(reader.GetValue(2)).Substring(0, 5)), Station = (string)reader.GetValue(3) });
                        }
                        catch (Exception) { }
            }
            catch (Exception ) { }
            finally { data.CloseConnection(); }
            return trainStations;
        }
        private List<Train> StartTrain()
        {
            List<Train> trainList = new List<Train>();
            string query = "SELECT NameTrain,Direction,Departure,Arrival,TrainType FROM Train;";
            SqlCommand command = new SqlCommand(query,data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        try
                        {
                            string[] city = ((string)reader.GetValue(1)).Replace(" - ", "!").Split('!');
                            DateTime departure = Convert.ToDateTime(Convert.ToString(reader.GetValue(2)).Substring(0, 5));
                            DateTime arrival = Convert.ToDateTime(Convert.ToString(reader.GetValue(3)).Substring(0,5));
                            trainList.Add(new Train((string)reader.GetValue(4),(string)reader.GetValue(0), (string)reader.GetValue(1), departure,city[0], arrival,city[1], arrival-departure));
                        }
                        catch (Exception) { }
            }
            catch (Exception ) { }
            return trainList;
        }
        private List<Train> GetTrains(List<TrainStation> first, List<TrainStation> second)
        {
            List<Train> trains = new List<Train>();
            foreach (var firstStation in first)
            {
                foreach (var secondStation in second)
                {
                    if (firstStation.TrainId == secondStation.TrainId && firstStation.Id < secondStation.Id)
                    {
                        string query = $"SELECT NameTrain,Direction,TrainType FROM Train WHERE Id = {firstStation.TrainId};";
                        SqlCommand command = new SqlCommand(query, data.GetConnection());
                        data.OpenConnection();
                        try
                        {
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                                while (reader.Read())
                                    try
                                    {
                                        trains.Add(new Train((string)reader.GetValue(2), (string)reader.GetValue(0), (string)reader.GetValue(1), firstStation.DTime, firstStation.Station, secondStation.DTime, secondStation.Station, secondStation.DTime - firstStation.DTime));
                                    }
                                    catch (Exception) { }
                        }
                        catch (Exception ex) { Console.WriteLine(ex); }
                        data.CloseConnection();
                    }
                }
            }
            return trains;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string query = $"SELECT Id,TrainId,Departure,Station FROM TrainRoute WHERE Station LIKE '%{FirstStation.Text}%';";
            List<TrainStation> firstStation = GetStation(query, FirstStation.Text);
            query = $"SELECT Id,TrainId,Direction,Station FROM TrainRoute WHERE Station LIKE '%{SecondStation.Text}%';";
            List<TrainStation> seconStation = GetStation(query, SecondStation.Text);
            Trains = GetTrains(firstStation, seconStation);
            ReserveListTrains = Trains.ToList();
            FlightsListBox.ItemsSource = Trains;
        }

        private bool DepartureSort { get; set; }
        private bool ArrivalSort { get; set; }
        private bool OnTheWaySort { get; set; }
        private void DepartureButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepartureSort)
            {
                DepartureButton.Content = "Прибытие↑";
                DepartureSort = false;
                Trains = Trains.OrderBy(x => x.departureTime).ToList();
                FlightsListBox.ItemsSource = Trains;
            }
            else
            {
                DepartureButton.Content = "Прибытие↓";
                DepartureSort = true;
                Trains=Trains.OrderByDescending(x => x.departureTime).ToList();
                FlightsListBox.ItemsSource = Trains;
            }
            OnTheWay.Content = "В пути";
            Arrival.Content = "Отправление";
        }

        private void Arrival_Click(object sender, RoutedEventArgs e)
        {
            if (ArrivalSort)
            {
                Arrival.Content = "Отправление↑";
                ArrivalSort = false;
                Trains=Trains.OrderBy(x => x.arrivalTime).ToList();
                FlightsListBox.ItemsSource = Trains;
            }
            else
            {
                Arrival.Content = "Отпрадение↓";
                ArrivalSort = true;
                Trains = Trains.OrderByDescending(x => x.arrivalTime).ToList();
                FlightsListBox.ItemsSource = Trains;
            }
            OnTheWay.Content = "В пути";
            DepartureButton.Content = "Прибытие";
        }

        private void OnTheWay_Click(object sender, RoutedEventArgs e)
        {
            if (OnTheWaySort)
            {
                OnTheWay.Content = "В пути↑";
                OnTheWaySort = false;
                Trains = Trains.OrderBy(x => x.durationTime).ToList();
                FlightsListBox.ItemsSource = Trains;
            }
            else
            {
                OnTheWay.Content = "В пути↓";
                OnTheWaySort = true;
                Trains = Trains.OrderByDescending(x => x.durationTime).ToList();
                FlightsListBox.ItemsSource = Trains;
            }
            Arrival.Content = "Отправление";
            DepartureButton.Content = "Прибытие";
        }

        private void Station_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)=>
            dataProcessing.CompanyProcessing(sender, e);

        private void GoToBucket_Click(object sender, RoutedEventArgs e)
        {
            TicketEvent?.Invoke("Добавить");
            if (SityBusPage.CountOfTickets > 0)
            {
                string [] date = DepartureTrain.Text.Split('.');
                string query = $"INSERT INTO ShoppingBasket(IdPersonalLoginData,TicketWhichTransport,RouteTicket,DepartureTime,DepartureDate,TicketStatus,CountTickets,TransportName)" +
                    $" VALUES((SELECT Id FROM PersonalLoginData WHERE Email = '{MainFrame.user.Email}'), 0, '{Trains[FlightsListBox.SelectedIndex].trainRoute}', '{((DateTime)Trains[FlightsListBox.SelectedIndex].departureTime).ToShortTimeString()}','{date[1]}-{date[0]}-{date[2]}', 0, {SityBusPage.CountOfTickets}, '{Trains[FlightsListBox.SelectedIndex].trainNumber}'); ";
                if (AddTicketIntoBD(query))
                    Notification?.Invoke("Билет добавлен в корзину, не забудьте оплатить");
                else
                    Notification?.Invoke("Возникла ошибка при добавлении билета");
            }
            MainFrame.BasketItemsCount += 1;
            MainFrame.basketButton.GetBindingExpression(TagProperty).UpdateTarget();
            Badged badged = (Badged)MainFrame.basketButton.Template.FindName("basketItemsCount", MainFrame.basketButton);
            badged.GetBindingExpression(Badged.BadgeProperty).UpdateTarget();
            SityBusPage.CountOfTickets = 0;
        }

        private void ByTicket_Click(object sender, RoutedEventArgs e)
        {
            TicketEvent?.Invoke("Купить");
            if (SityBusPage.CountOfTickets > 0)
            {
                Warning?.Invoke("Вы уверены, что хотите приобрести билет?", "Купить");
                if (SityBusPage.ConfirmBuyTicket)
                {
                    string[] date = DepartureTrain.Text.Split('.');
                    string query = $"INSERT INTO ShoppingBasket(IdPersonalLoginData,TicketWhichTransport,RouteTicket,DepartureTime,DepartureDate,TicketStatus,CountTickets,TransportName)" +
                    $" VALUES((SELECT Id FROM PersonalLoginData WHERE Email = '{MainFrame.user.Email}'), 0, '{Trains[FlightsListBox.SelectedIndex].trainRoute}', '{((DateTime)Trains[FlightsListBox.SelectedIndex].departureTime).ToShortTimeString()}','{date[1]}-{date[0]}-{date[2]}', 1, {SityBusPage.CountOfTickets}, '{Trains[FlightsListBox.SelectedIndex].trainNumber}'); ";
                    if (AddTicketIntoBD(query))
                    {
                        Notification?.Invoke("Спасибо за покупку. Билет у вас в корзине");
                        MainFrame.BasketItemsCount += 1;
                        MainFrame.basketButton.GetBindingExpression(TagProperty).UpdateTarget();
                        Badged badged = (Badged)MainFrame.basketButton.Template.FindName("basketItemsCount", MainFrame.basketButton);
                        badged.GetBindingExpression(Badged.BadgeProperty).UpdateTarget();
                    }
                    else
                        Notification?.Invoke("Возникла ошибка при покупке билета");
                }
                SityBusPage.ConfirmBuyTicket = false;
            }
            SityBusPage.CountOfTickets = 0;
        }
        private bool AddTicketIntoBD(string query)
        {
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try { command.ExecuteNonQuery(); }
            catch (Exception) { return false; }
            finally { data.CloseConnection(); }
            return true;
        }
        private void FlightsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ByTicket.Visibility = Visibility.Visible;
            GoToBucket.Visibility = Visibility.Visible;
        }

        private void DepartureTrain_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)=>
            dataProcessing.Birthday(sender, e);

        private List<Train> ReserveListType { get; set; }=new List<Train>();

        private void AllType_Click(object sender, RoutedEventArgs e)
        {
            if (ReserveListType.Count==0)
                ReserveListType = Trains.ToList();
            if (((TextBlock)((RadioButton)sender).Content).Text == "Все")
            {
                Trains = ReserveListTrains.ToList();
                FlightsListBox.ItemsSource = Trains;
                ReserveListType.Clear();
            }
            else
            {
                Trains = ReserveListType.ToList();
                Trains = Trains.Where(x => x.trainType == ((TextBlock)((RadioButton)sender).Content).Text).ToList();
                FlightsListBox.ItemsSource = Trains;
            }
        }
        private List<Train> ReserveListDeparture { get; set; } = new List<Train>();
        private bool _depArrival { get; set; }
        private void DepArrival_Click(object sender, RoutedEventArgs e)
        {
            if ((string)((RadioButton)sender).Content == "Отправление")
                _depArrival = true;
            else if((string)((RadioButton)sender).Content == "Прибытие")
                _depArrival= false;
        }
        private void TimeArrival_Click(object sender, RoutedEventArgs e)
        {
            if (ReserveListDeparture.Count==0)
                ReserveListDeparture = Trains.ToList();
            Trains = ReserveListDeparture.ToList();
            if (!_depArrival)
            {
                if ((string)((RadioButton)sender).Content == "Утро (с 06.00 - 11.59)")
                    Trains = Trains.Where(x => x.departureTime.Hour >= 06 && x.departureTime.Hour < 12).ToList();
                else if ((string)((RadioButton)sender).Content == "День (с 12.00 - 17.59)")
                    Trains = Trains.Where(x => x.departureTime.Hour >= 12 && x.departureTime.Hour < 18).ToList();
                else if ((string)((RadioButton)sender).Content == "Вечер (с 18.00 - 23.59)")
                    Trains = Trains.Where(x => x.departureTime.Hour >= 18 && x.departureTime.Hour <= 23).ToList();
                else if ((string)((RadioButton)sender).Content == "Ночь (с 00.00 - 05.59)")
                    Trains = Trains.Where(x => x.departureTime.Hour >= 00 && x.departureTime.Hour < 6).ToList();
            }
            else
            {
                if ((string)((RadioButton)sender).Content == "Утро (с 06.00 - 11.59)")
                    Trains = Trains.Where(x => x.arrivalTime.Hour >= 06 && x.arrivalTime.Hour < 12).ToList();
                else if ((string)((RadioButton)sender).Content == "День (с 12.00 - 17.59)")
                    Trains = Trains.Where(x => x.arrivalTime.Hour >= 12 && x.arrivalTime.Hour < 18).ToList();
                else if ((string)((RadioButton)sender).Content == "Вечер (с 18.00 - 23.59)")
                    Trains = Trains.Where(x => x.arrivalTime.Hour >= 18 && x.arrivalTime.Hour <= 23).ToList();
                else if ((string)((RadioButton)sender).Content == "Ночь (с 00.00 - 05.59)")
                    Trains = Trains.Where(x => x.arrivalTime.Hour >= 00 && x.arrivalTime.Hour < 6).ToList();
            }
            FlightsListBox.ItemsSource = Trains;
        }
    }
}
