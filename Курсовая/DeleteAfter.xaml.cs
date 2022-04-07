using System;
using System.Windows.Controls;
using System.Windows.Threading;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;

namespace Курсовая
{
    /// <summary>
    /// Логика взаимодействия для DeleteAfter.xaml
    /// </summary>
    public partial class DeleteAfter : Page
    {
        private int _mm=9, _ss=60;
        private bool _ok=true;

        
        public DeleteAfter()
        {
            InitializeComponent();
            StartClock();
            StartTimer();
            WeatherInfo();
            Blackout.End = DateTime.Now.AddDays(-1);
        }


        private void WeatherInfo()
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q=Pinsk&units=metric&appid=5aeecb8f638755cf1590123b55b8a2bc";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            using(StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                response = streamReader.ReadToEnd();
            Weather weather = JsonConvert.DeserializeObject<Weather>(response);
            WeatherShow.Text = Math.Round(weather.Main.Temp) + " °C " + weather.Name;
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

        private void Clock_Tick(object sender, EventArgs e)=>
            RealTime.Text = DateTime.Now.ToString(@"hh\:mm\:ss");


        private void StartTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _ss--;
            if (_ss == 00)
            {
                _ss = 59;
                _mm--;
                if (_mm == -1)
                {
                    _ok = false;
                    TimerCode.Text = "Время вышло";
                    ((DispatcherTimer)sender).Stop();
                }
            }
            TimerCode.Text = new DateTime(2003, 10, 20, 00, _mm, _ss).ToString(@"mm\:ss");
        }

       
    }
}
