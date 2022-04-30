using ProfileClassLibrary.PlaneClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
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
using System.Data.SqlClient;
using System.Data;
using Курсовая.Setting;
using Newtonsoft.Json;
using Курсовая.MainFrameForms.PlanePages;
using Курсовая.MainFrameForms.SityBusPages;

namespace Курсовая.MainFrameForms
{
    /// <summary>
    /// Логика взаимодействия для PlanePage.xaml
    /// </summary>
    public partial class PlanePage : Page
    {
        public static string FlightClassString;
        public static int PeopleCount { get; set; }
        public static int AllLuggageCount { get; set; }
        private DataBase data;
        public PlanePage()
        {
            InitializeComponent();
            DataContext = this;
            DayComboBox.SelectedIndex = 3;

            data = new DataBase();
            ChangingDatePlane();

            DataContext = this;
        }
        public List<Plane> planes { get; set; } = new List<Plane>() {
            new Plane() {
                ImagePath= "https://airport.by/upload/images/4e3bdc6556844bde1c84c53fc65a8cc6.png",
                DepartureTime = "06:20",
                Direction = "Москва(Ithtvtnmtdj)",
                FlightName="B214",
                Gate="12",
                Status = "Регистрация" },
            new Plane() {
                ImagePath= "https://airport.by/upload/images/4e3bdc6556844bde1c84c53fc65a8cc6.png",
                DepartureTime = "06:20",
                Direction = "Москва",
                FlightName="B214",
                Gate="12",
                Status = "Регистрация"  } };



        private void FlightsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuyButtons.Visibility = Visibility.Visible;
        }
        private List<Flight> ReadingPlaneFromNet()
        {
            List<Flight> flights = new List<Flight>();
            try
            {
                byte[] bytes = new byte[1000];
                HttpWebRequest rq = (HttpWebRequest)WebRequest.Create("https://airport.by/ru/flights/departure");
                rq.Method = "POST";
                rq.ContentLength = bytes.Length;
                rq.ContentType = "text/json; encoding='utf-8'";
                using (Stream str = rq.GetRequestStream())
                    str.Write(bytes, 0, bytes.Length);
                HttpWebResponse httpWebResponse = (HttpWebResponse)rq.GetResponse();
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    DataContractJsonSerializer dataContractJson = new DataContractJsonSerializer(typeof(List<Flight>));
                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                        flights = (List<Flight>)dataContractJson.ReadObject(responseStream);
                }
                httpWebResponse.Close();
            }
            catch (Exception) { }
            return flights;
        }
        private void FlightClass_Click(object sender, RoutedEventArgs e)
        {
            FlightClassString = (string)FlightClass.Content;
            ClassChoiceWindow classChoiceWindow = new ClassChoiceWindow();
            classChoiceWindow.Left = PointToScreen(Mouse.GetPosition(this)).X - Mouse.GetPosition(this).X;
            classChoiceWindow.Top = PointToScreen(Mouse.GetPosition(this)).Y - Mouse.GetPosition(this).Y;

            classChoiceWindow.ShowDialog();

            FlightClass.Content = FlightClassString;
        }
        private void InsertDataInBD(List<Flight> flights)
        {
            data.OpenConnection();
            Random rnd = new Random();
            foreach (var item in flights)
            {
                bool access = default;
                string query = $"INSERT INTO Airline(Company) VALUES('{item.airline.title}'); ";
                SqlCommand command = new SqlCommand(query, data.GetConnection());
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable dataTable = new DataTable();
                try
                {
                    try

                    {
                        if (command.ExecuteNonQuery() > 0)
                            access = true;
                    }
                    catch
                    {
                        query = $"SELECT * FROM Airline WHERE Company ='{item.airline.title}';";
                        command = new SqlCommand(query, data.GetConnection());
                        adapter.SelectCommand = command;
                        adapter.Fill(dataTable);
                        if (dataTable.Rows.Count > 0)
                            access = true;
                    }
                    if (access && item.status.title != "Вылетел")
                    {
                        if (item.numbers_gate.Length != 0)
                        {
                            query = $"UPDATE Plane SET Landing = '{item.numbers_gate[0]}' WHERE Flight = '{item.flight}'" +
                                  $" AND Direction ='{item.airport.title}' AND DepartureTime ='{Convert.ToDateTime(item.plan).ToString(@"HH\:mm")}' " +
                                  $"AND DepartureDate ='{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}';";
                            access = false;
                        }
                        //query = $"INSERT INTO Plane(AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate) " +
                        //     $"VALUES((SELECT ID FROM Airline WHERE Company = '{item.airline.title}'),'{item.flight}','{item.airport.title}','{item.numbers_gate[0]}'," +
                        //     $"'{Convert.ToDateTime(item.plan).ToString(@"HH\:mm")}','{item.status.title}','{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}'); ";
                        //else
                        else if (item.numbers_gate.Length == 0)
                            query = $"INSERT INTO Plane(AirlineId,Flight,Direction,DepartureTime,StatusPlane,DepartureDate) " +
                                       $"VALUES((SELECT ID FROM Airline WHERE Company = '{item.airline.title}'),'{item.flight}','{item.airport.title}'," +
                                       $"'{Convert.ToDateTime(item.plan).ToString(@"HH\:mm")}','{item.status.title}'," +
                                       $"'{DateTime.Now.Month}-{DateTime.Now.AddDays(1).Day}-{DateTime.Now.Year}'); ";
                        command = new SqlCommand(query, data.GetConnection());
                        if (command.ExecuteNonQuery() > 0 && access)
                        {
                            try
                            {
                                //query = $"INSERT INTO NumberSeats(PlaneId,BusinessClass,EconomyClass,PremiumEconomyClass,FirstClass) " +
                                //    $"VALUES((SELECT ID FROM Plane WHERE Flight = '{item.flight}' AND StatusPlane='{item.status.title}' " +
                                //    $"AND DepartureTime ='{Convert.ToDateTime(item.plan).ToString(@"HH\:mm")}' AND Landing='{item.numbers_gate[0]}' " +
                                //    $"AND Direction='{item.airport.title}'), {rnd.Next(0, 30)},{rnd.Next(0, 80)},{rnd.Next(0, 20)},{rnd.Next(0, 50)}); ";
                                query = $"INSERT INTO NumberSeats(PlaneId,BusinessClass,EconomyClass,PremiumEconomyClass,FirstClass) " +
                                    $"VALUES((SELECT ID FROM Plane WHERE Flight = '{item.flight}' AND StatusPlane='{item.status.title}' " +
                                    $"AND DepartureTime ='{Convert.ToDateTime(item.plan).ToString(@"HH\:mm")}' AND Landing IS NULL AND Direction='{item.airport.title}')," +
                                    $"{rnd.Next(0, 30)},{rnd.Next(0, 80)},{rnd.Next(0, 20)},{rnd.Next(0, 50)}); ";
                            }
                            catch (Exception)
                            {
                                query = $"INSERT INTO NumberSeats(PlaneId,BusinessClass,EconomyClass,PremiumEconomyClass,FirstClass) " +
                                    $"VALUES((SELECT ID FROM Plane WHERE Flight = '{item.flight}' AND StatusPlane='{item.status.title}' " +
                                    $"AND DepartureTime ='{Convert.ToDateTime(item.plan).ToString(@"HH\:mm")}' AND Landing IS NULL AND Direction='{item.airport.title}')," +
                                    $"{rnd.Next(0, 30)},{rnd.Next(0, 80)},{rnd.Next(0, 20)},{rnd.Next(0, 50)}); ";
                            }
                            command = new SqlCommand(query, data.GetConnection());
                            command.ExecuteNonQuery();
                            
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            data.CloseConnection();

        }
        private void ChangingDatePlane()
        {
            QueryBD($"DELETE FROM Plane WHERE DepartureTime< '{DateTime.Now.ToString(@"HH\:mm")}' AND Landing IS NOT NULL  " +
                $"AND DepartureDate <= '{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}';");
            string query = $"SELECT * FROM Plane WHERE Landing IS NOT NULL;";
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            data.OpenConnection();
            try
            {
                adapter.SelectCommand = command;
                adapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    QueryBD($"UPDATE Plane SET StatusPlane = 'Регистрация' WHERE DepartureTime < '{DateTime.Now.AddHours(1).AddMinutes(30).ToString(@"HH\:mm")}' " +
                        $"AND Landing IS NOT NULL;");
                    QueryBD($"UPDATE Plane SET StatusPlane = 'Посадка' WHERE DepartureTime < '{DateTime.Now.AddMinutes(45).ToString(@"HH\:mm")}' AND Landing IS NOT NULL;");
                }
                else
                    InsertDataInBD(ReadingPlaneFromNet());
               
            }
            catch (Exception) { }
            finally { data.CloseConnection(); }

        }
        private void FlightSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            FlightSettingsWindow flightSettingsWindow = new FlightSettingsWindow();
            flightSettingsWindow.ShowDialog();
            TextBlock PeopleTb = (TextBlock)FlightSettingsBtn.Template.FindName("People",FlightSettingsBtn);
            PeopleTb.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            TextBlock LuggageTb = (TextBlock)FlightSettingsBtn.Template.FindName("Luggage", FlightSettingsBtn);
            LuggageTb.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }
        private void QueryBD(string query)
        {
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try { command.ExecuteNonQuery(); }
            catch (Exception) { }
            finally { data.CloseConnection(); }
        }
        public class Plane
        {
            public string ImagePath { get; set; }
            public string FlightName { get; set; }
            public string Direction { get; set; }
            public string Gate { get; set; }
            public string DepartureTime { get; set; }
            public string Status { get; set; }

        }
    }
}
