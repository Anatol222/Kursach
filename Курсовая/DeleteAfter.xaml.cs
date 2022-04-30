using System;
using System.Windows.Controls;
using System.Windows.Threading;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;
using ProfileClassLibrary;
using System.Data.SqlClient;
using System.Data;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;

namespace Курсовая
{
    public partial class DeleteAfter : Page
    {
        //private DataBase dataBase;
        //private CipherPassword cipherPassword;
        //private IRegComeIn regComeIn;
        //private INavigation navigation;
        //private IDataProcessing dataProcessing;

        public DeleteAfter()
        {
            InitializeComponent();
            //StartClock();
            Blackout.End = DateTime.Now.AddDays(-1);
            //WeatherInfo();
            //dataBase = new DataBase();
            //regComeIn = new RegComeIn();
            //navigation = new ProgrammNavigation();
            //dataProcessing = new DataProcessing();
            //cipherPassword = new CipherPassword();
        }


        //private void WeatherInfo()
        //{
        //    string url = $"https://api.openweathermap.org/data/2.5/weather?q=Pinsk&units=metric&appid=5aeecb8f638755cf1590123b55b8a2bc";
        //    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //    string response;
        //    using(StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
        //        response = streamReader.ReadToEnd();
        //    Weather weather = JsonConvert.DeserializeObject<Weather>(response);
        //    WeatherShow.Text = Math.Round(weather.Main.Temp) + " °C " + weather.Name;
        //    Image image = new Image();
        //    BitmapImage bitmap = new BitmapImage();
        //    bitmap.BeginInit();
        //    bitmap.UriSource = new Uri($"Images/ImagesWeather/{weather.weather[0].Icon}.png", UriKind.Relative);
        //    bitmap.EndInit();
        //    image.Source = bitmap;
        //    IconWeatherShow.Content = image;
        //}

        //private void StartClock()
        //{
        //    DispatcherTimer timer = new DispatcherTimer();
        //    timer.Interval = TimeSpan.FromSeconds(1);
        //    timer.Tick += Clock_Tick;
        //    timer.Start();
        //}

        //private void Clock_Tick(object sender, EventArgs e) =>
        //    RealTime.Text = DateTime.Now.ToString(@"hh\:mm\:ss");


        //private void UserData_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    User user = new User("gorboveckirill@gmail.com");
        //    string querystring = $"SELECT UPD.FirstName,UPD.LastName,UPD.Patronymic,UPD.Number,UPD.Birthday,UPD.Gender,PLD.ReserveEmail,PP.Password FROM UserPersonalData AS UPD,PersonalLoginData AS PLD,PersonalPassword AS PP WHERE UPD.Id=PP.UserPersonalDataId AND UPD.Id = PLD.UserPersonalDataId AND PLD.Email='{_email}'; ";
        //    SqlCommand sqlCommand = new SqlCommand(querystring, dataBase.GetConnection());
        //    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        //    try
        //    {
        //        while (sqlDataReader.Read())
        //        {
        //            _lastName = Convert.ToString(sqlDataReader["LastName"]);
        //            _firstname = Convert.ToString(sqlDataReader["FirstName"]);
        //            _patronimic = Convert.ToString(sqlDataReader["Patronymic"]);
        //            _number = Convert.ToString(sqlDataReader["Number"]);
        //            _birthday = Convert.ToString(sqlDataReader["Birthday"]);
        //            _gender = Convert.ToString(sqlDataReader["Gender"]);
        //            _reserveEmail = Convert.ToString(sqlDataReader["ReserveEmail"]);
        //            _password = cipherPassword.decode(Convert.ToString(sqlDataReader["Password"]));
        //        }
        //        //read = sqlDataReader.GetString(0);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
    }
}
