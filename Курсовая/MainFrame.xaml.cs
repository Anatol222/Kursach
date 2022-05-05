using Newtonsoft.Json;
using ProfileClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Курсовая.MainFrameForms;
using Курсовая.Setting;

namespace Курсовая
{
    public partial class MainFrame : Window
    {
        public static MainFrame mainFrame;
        public static User user;
        public static RadioButton basketButton;
        Uri BusketPage = new Uri("MainFrameForms/BucketPage.xaml", UriKind.RelativeOrAbsolute);
        Uri SityBusPage = new Uri("MainFrameForms/SityBusPage.xaml", UriKind.RelativeOrAbsolute);
        Uri BusPage = new Uri("MainFrameForms/BusPage.xaml", UriKind.RelativeOrAbsolute);
        Uri PlanePage = new Uri("MainFrameForms/PlanePage.xaml", UriKind.RelativeOrAbsolute);
        Uri TrainPage = new Uri("MainFrameForms/TrainPage.xaml", UriKind.RelativeOrAbsolute);
        Uri ProfilePage = new Uri("MainFrameForms/ProfilePage.xaml", UriKind.RelativeOrAbsolute);
       
        private string _email { get; }
        private bool ALTtrue;
        public MainFrame()
        {

            InitializeComponent();
            DataContext = this;
            ChangeData();
            BasketItemsCount = CountTicketsInBasket();
        }
        public static int BasketItemsCount { get; set; }
        public MainFrame(string email)
        {
            InitializeComponent();
            DataContext = this;
            _email = email;
            user = new User(_email);

            basketButton = Bucket;

            BasketItemsCount = CountTicketsInBasket();
            ChangeData();
        }
        public void ChangeData()
        {
            WeatherInfo();
            StartClock();
            mainFrame = this;
            FNamePatronymic.Content = user.Name + " " + user.Patronymic;
            EmailUres.Content = user.Email;
            if (File.Exists("UserIcon.json"))
            {
                DataContractJsonSerializer jsonF = new DataContractJsonSerializer(typeof(List<IconUser>));
                List<IconUser> dataUserSave = new List<IconUser>();
                using (FileStream fs = new FileStream("UserIcon.json", FileMode.Open))
                    dataUserSave = (List<IconUser>)jsonF.ReadObject(fs);
                Image image = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(dataUserSave[0].IconAn, UriKind.Relative);
                bitmap.EndInit();
                image.Source = bitmap;
                OpenSetting.Content = image;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>
            this.DragMove();

        private void MainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(SityBusPage);
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)=>
            WindowState = WindowState.Minimized;
        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }
        private void Close_Click(object sender, RoutedEventArgs e) =>
            Close();

        private void Menu_Click(object sender, RoutedEventArgs e) =>
            ChangeData();
        private void Plane_Click(object sender, RoutedEventArgs e)=>
            PagesNavigation.Navigate(PlanePage);
        private void Train_Click(object sender, RoutedEventArgs e)=>
            PagesNavigation.Navigate(TrainPage);
        private void SityBus_Click(object sender, RoutedEventArgs e) =>
           PagesNavigation.Navigate(SityBusPage);
        private void Bus_Click(object sender, RoutedEventArgs e)=>
            PagesNavigation.Navigate(BusPage);
        private void Bucket_Click(object sender, RoutedEventArgs e) =>
            PagesNavigation.Navigate(BusketPage);
        private void OpenSetting_Click(object sender, RoutedEventArgs e) =>
            PagesNavigation.Navigate(ProfilePage);

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (WindowState == WindowState.Normal)
                    WindowState = WindowState.Maximized;
                else
                    WindowState = WindowState.Normal;
            }
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl || ALTtrue == true)
            {
                if (e.Key == Key.Q)
                    Close();
                else if (e.Key == Key.M)
                    WindowState = WindowState.Minimized;
                else if (e.Key == Key.D1)
                    PagesNavigation.Navigate(PlanePage);
                else if (e.Key == Key.D2)
                    PagesNavigation.Navigate(TrainPage);
                else if (e.Key == Key.D3)
                    PagesNavigation.Navigate(SityBusPage);
                else if (e.Key == Key.D4)
                    PagesNavigation.Navigate(BusPage);
                else if (e.Key == Key.D5)
                    PagesNavigation.Navigate(BusketPage);
                else if (e.Key == Key.D6)
                    PagesNavigation.Navigate(ProfilePage);
                ALTtrue = !ALTtrue;
            }
        }
        private void WeatherInfo()
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q=Pinsk&units=metric&appid=5aeecb8f638755cf1590123b55b8a2bc";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                response = streamReader.ReadToEnd();
            Weather weather = JsonConvert.DeserializeObject<Weather>(response);
            WeatherShow.Text = Math.Round(weather.Main.Temp) + " °C " + weather.Name+" ";
            Image image = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri($"Images/ImagesWeather/{weather.weather[0].Icon}.png", UriKind.Relative);
            bitmap.EndInit();
            image.Source = bitmap;
            IconWeatherShow.Content = image;
        }
        private void StartClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Clock_Tick;
            timer.Start();
        }

        private void Clock_Tick(object sender, EventArgs e) =>
            RealTime.Text = DateTime.Now.ToString(@"HH\:mm\:ss");

        private int CountTicketsInBasket()
        {
            int countTickets = default;
            DataBase data = new DataBase();
            string query = $"SELECT TicketWhichTransport FROM ShoppingBasket WHERE IdPersonalLoginData = (SELECT Id FROM PersonalLoginData WHERE Email = '{user.Email}');";
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            SqlDataAdapter adapter= new SqlDataAdapter();
            DataTable table = new DataTable();
            data.OpenConnection();
            try
            {
                adapter.SelectCommand = command;
                adapter.Fill(table);
                countTickets = table.Rows.Count;
            }
            catch (Exception) {}
            finally { data.CloseConnection(); }
            return countTickets;
        }
    }
}
