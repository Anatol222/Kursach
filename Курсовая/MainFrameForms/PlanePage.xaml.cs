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
using Курсовая.ProgrammInterface;

namespace Курсовая.MainFrameForms
{
    public partial class PlanePage : Page
    {
        public static string FlightClassString;
        public static int PeopleCount { get; set; }
        public static int AllLuggageCount { get; set; }
        private DataBase data;
        private IDataProcessing dataProcessing;
        private INavigation navigation;

        private delegate void InvoceMessageBox(string messange);
        private event InvoceMessageBox Notification;
        public PlanePage()
        {
            InitializeComponent();
            DataContext = this;
            DayComboBox.SelectedIndex = 3;


            navigation = new ProgrammNavigation();
            dataProcessing = new DataProcessing();
            data = new DataBase();
            ChangingDatePlane();

            Notification += navigation.Display;
            
            Planes = GetDateFromBD();
            FillingChoiceCountry();

            ChoiceCountry.SelectedIndex = 0;
        }
        public List<Plane> Planes { get; set; }
        private void FillingChoiceCountry()
        {
            string query = "SELECT DISTINCT Direction FROM Plane";
            SqlCommand command = new SqlCommand(query,data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    ChoiceCountry.Items.Add("Любая");
                    while (reader.Read())
                        ChoiceCountry.Items.Add((string)reader.GetValue(0));
                    reader.Close();
                }
            }
            catch (Exception) {}
            finally { data.CloseConnection(); }

        }

        private List<Plane> GetDateFromBD()
        {
            List<Plane> planes = new List<Plane>();
            string query = "SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate FROM Plane;";
            SqlCommand command = new SqlCommand(query,data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        planes.Add(new Plane(Convert.ToInt32(reader.GetValue(0)))
                        {
                            FlightName = (string)reader.GetValue(1),
                            Direction = (string)reader.GetValue(2),
                            Gate = Convert.ToString(reader.GetValue(3)),
                            DepartureTime = Convert.ToDateTime(Convert.ToString(reader.GetValue(4)).Substring(0,5)),
                            Status = (string)reader.GetValue(5),
                            DepartureDate = Convert.ToDateTime(reader.GetValue(6)),
                        });
                    }
                    reader.Close();
                }
            }
            catch (Exception) { }
            finally { data.CloseConnection(); }
            return planes;
        }
        private void FlightsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ByTicket.Visibility = Visibility.Visible;
            GoToBucket.Visibility = Visibility.Visible;
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
        private void FlightSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            FlightSettingsWindow flightSettingsWindow = new FlightSettingsWindow();
            flightSettingsWindow.Show();
        }
        private void InsertDataInBD(List<Flight> flights)
        {
            data.OpenConnection();
            Random rnd = new Random();
            string query = default;
            SqlCommand command = default;
            foreach (var item in flights)
            {
                bool access = default;
                query = $"INSERT INTO Airline(Company) VALUES('{item.airline.title}'); ";
                command = new SqlCommand(query, data.GetConnection());
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
                        else if (item.numbers_gate.Length == 0)
                        {
                            DateTime date = DateTime.Now.AddDays(1);
                            query = $"INSERT INTO Plane(AirlineId,Flight,Direction,DepartureTime,StatusPlane,DepartureDate) " +
                                         $"VALUES((SELECT ID FROM Airline WHERE Company = '{item.airline.title}'),'{item.flight}','{item.airport.title}'," +
                                         $"'{Convert.ToDateTime(item.plan).ToString(@"HH\:mm")}','{item.status.title}'," +
                                         $"'{((DateTime)date).Month}-{((DateTime)date).Day}-{((DateTime)date).Year}'); ";
                        }
                        command = new SqlCommand(query, data.GetConnection());
                        if (command.ExecuteNonQuery() > 0 && access)
                        {
                            try
                            {
                                query = $"INSERT INTO NumberSeats(PlaneId,BusinessClass,EconomyClass,PremiumEconomyClass,FirstClass) " +
                                    $"VALUES((SELECT ID FROM Plane WHERE Flight = '{item.flight}' AND StatusPlane='{item.status.title}' " +
                                    $"AND DepartureTime ='{Convert.ToDateTime(item.plan).ToString(@"HH\:mm")}' AND Landing IS NULL AND Direction='{item.airport.title}')," +
                                    $"{rnd.Next(0, 30)},{rnd.Next(0, 80)},{rnd.Next(0, 20)},{rnd.Next(0, 50)}); ";
                            }
                            catch (Exception) { }
                            command = new SqlCommand(query, data.GetConnection());
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception) { }
            }
            query = $"DELETE FROM Plane WHERE Landing IS NULL AND DepartureDate = '{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}';";
            command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try { command.ExecuteNonQuery(); }
            catch (Exception){ }
            data.CloseConnection();
        }
        private void ChangingDatePlane()
        {
            QueryBD($"DELETE FROM Plane WHERE (DepartureTime< '{DateTime.Now.ToString(@"HH\:mm")}' OR DepartureDate < '{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}')" +
                $" AND Landing IS NOT NULL ;");
            string query = $"SELECT * FROM Plane WHERE Landing IS NOT NULL;";
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            data.OpenConnection();
            try
            {
                adapter.SelectCommand = command;
                adapter.Fill(dataTable);
                if (dataTable.Rows.Count <= 0)
                    InsertDataInBD(ReadingPlaneFromNet());
                QueryBD($"UPDATE Plane SET StatusPlane = 'Регистрация' WHERE DepartureTime < '{DateTime.Now.AddHours(1).AddMinutes(30).ToString(@"HH\:mm")}' " +
                        $"AND Landing IS NOT NULL;");
                QueryBD($"UPDATE Plane SET StatusPlane = 'Посадка' WHERE DepartureTime < '{DateTime.Now.AddMinutes(45).ToString(@"HH\:mm")}' AND Landing IS NOT NULL;");

            }
            catch (Exception) { }
            finally { data.CloseConnection(); }
        }

        }
        private void FlightSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            FlightSettingsWindow flightSettingsWindow = new FlightSettingsWindow();
            flightSettingsWindow.ShowDialog();
            TextBlock PeopleTb = (TextBlock)FlightSettingsBtn.Template.FindName("People",FlightSettingsBtn);
            PeopleTb.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            TextBlock LuggageTb = (TextBlock)FlightSettingsBtn.Template.FindName("Luggage", FlightSettingsBtn);
            LuggageTb.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
=========
>>>>>>>>> Temporary merge branch 2
        }

        }
        private void FlightSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            FlightSettingsWindow flightSettingsWindow = new FlightSettingsWindow();
            flightSettingsWindow.ShowDialog();
            TextBlock PeopleTb = (TextBlock)FlightSettingsBtn.Template.FindName("People",FlightSettingsBtn);
            PeopleTb.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            TextBlock LuggageTb = (TextBlock)FlightSettingsBtn.Template.FindName("Luggage", FlightSettingsBtn);
            LuggageTb.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
=========
>>>>>>>>> Temporary merge branch 2
        }

        }
        private void FlightSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            FlightSettingsWindow flightSettingsWindow = new FlightSettingsWindow();
            flightSettingsWindow.ShowDialog();
            TextBlock PeopleTb = (TextBlock)FlightSettingsBtn.Template.FindName("People",FlightSettingsBtn);
            PeopleTb.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            TextBlock LuggageTb = (TextBlock)FlightSettingsBtn.Template.FindName("Luggage", FlightSettingsBtn);
            LuggageTb.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
=========
>>>>>>>>> Temporary merge branch 2
        }
        private void QueryBD(string query)
        {
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try { command.ExecuteNonQuery(); }
            catch (Exception) { }
            finally { data.CloseConnection(); }
        }

        private void ChoiceCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{ChoiceCountry.SelectedItem.ToString()}%'" +
                $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%';";
            if (AirlineBox.Text != "")
            {
                query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{ChoiceCountry.SelectedItem.ToString()}%'" +
                  $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' " +
                  $"AND (AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%') OR AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%' ORDER BY Id DESC));";
            }
            if (ChoiceCountry.SelectedItem.ToString() == "Любая")
                query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate FROM Plane;";
            InvokeBD(query);
            if (ChoiceCountry.SelectedItem.ToString() != "Любая")
                DirectionBox.Text = ChoiceCountry.SelectedItem.ToString();
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
        private void InvokeBD(string query)
        {
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    Planes = new List<Plane>();
                    while (reader.Read())
                    {
                        Planes.Add(new Plane(Convert.ToInt32(reader.GetValue(0)))
                        {
                            FlightName = (string)reader.GetValue(1),
                            Direction = (string)reader.GetValue(2),
                            Gate = Convert.ToString(reader.GetValue(3)),
                            DepartureTime = Convert.ToDateTime(Convert.ToString(reader.GetValue(4)).Substring(0, 5)),
                            Status = (string)reader.GetValue(5),
                            DepartureDate = Convert.ToDateTime(reader.GetValue(6)),
                        });
                    }
                    reader.Close();
                }
            }
            catch (Exception) { }
            finally { data.CloseConnection(); }
            FlightsListBox.ItemsSource = Planes;
        }

        private void DirectionBox_PreviewTextInput(object sender, TextCompositionEventArgs e)=>
            dataProcessing.CompanyProcessing(sender, e);
        private void FlightBox_PreviewTextInput(object sender, TextCompositionEventArgs e)=>
            dataProcessing.NameProcessing(sender, e);
        private void TimeDeparture_PreviewTextInput(object sender, TextCompositionEventArgs e)=>
            dataProcessing.TimeProcessing(sender, e);

        private void Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{DirectionBox.Text}%'" +
                $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%';";
            if (AirlineBox.Text!="")
                query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{DirectionBox.Text}%'" +
                $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' " +
                $"AND (AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%') OR AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%' ORDER BY Id DESC));";
            InvokeBD(query);
        }
        private void DirectionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DirectionBox.Text!=ChoiceCountry.SelectedItem.ToString())
            {
                string query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{DirectionBox.Text}%'" +
                $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%';";
                if (AirlineBox.Text != "")
                    query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{DirectionBox.Text}%'" +
                    $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' " +
                    $"AND (AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%') OR AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%' ORDER BY Id DESC));";
                InvokeBD(query);
                ChoiceCountry.SelectedIndex = 0;
            }
        }
        private void AirlineBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           string query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{DirectionBox.Text}%'" +
                $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' " +
                $"AND (AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%') OR AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%' ORDER BY Id DESC));";
            InvokeBD(query);
        }


        private void GoToBucket_Click(object sender, RoutedEventArgs e)
        {
            string query = $"INSERT INTO ShoppingBasket(IdPersonalLoginData,TicketWhichTransport,RouteTicket,DepartureTime,DepartureDate,TicketStatus,CountTickets,TransportName,TypeService)" +
                $"VALUES((SELECT Id FROM PersonalLoginData WHERE Email = '{MainFrame.user.Email}'), '1', '{Planes[FlightsListBox.SelectedIndex].Direction}'," +
                $" '{Planes[FlightsListBox.SelectedIndex].DepartureTime.ToString(@"HH\:mm")}', '{Planes[FlightsListBox.SelectedIndex].DepartureDate.Month}-{Planes[FlightsListBox.SelectedIndex].DepartureDate.Day}-{Planes[FlightsListBox.SelectedIndex].DepartureDate.Year}', " +
                $"0, 2, '{Planes[FlightsListBox.SelectedIndex].FlightName}', '{(string)FlightClass.Content}'); ";
            if (AddTicketIntoBD(query))
                Notification?.Invoke("Билет добавлен в корзину, не забудьте оплатить");
            else
                Notification?.Invoke("Билетов в таком количестве нет");
        }

        private void ByTicket_Click(object sender, RoutedEventArgs e)
        {
            //TicketEvent?.Invoke("Купить");
            //Warning?.Invoke("Вы уверены, что хотите приобрести билет?", "Купить");
            //if (ConfirmBuyTicket)
            //{
            //    string query = "INSERT INTO ShoppingBasket (IdPersonalLoginData,TicketWhichTransport,RouteTicket,TicketStatus,CountTickets,TransportName) " +
            //     $"VALUES((SELECT Id FROM PersonalLoginData WHERE Email = '{MainFrame.user.Email}'),2,'{BusTimePage.aboutBus.Route}',1,{CountOfTickets},'{BusTimePage.aboutBus.BusName}');";
            //    if (AddTicketIntoBD(query))
            //        Notification?.Invoke("Спасибо за покупку. Билет у вас в корзине");
            //    else
            //        Notification?.Invoke("Возникла ошибка при покупке билета");
            //}
            //ConfirmBuyTicket = false;
            //CountOfTickets = 0;
        }
        private bool AddTicketIntoBD(string query)
        {
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try { command.ExecuteNonQuery(); }
            catch(Exception) { return false; }
            finally { data.CloseConnection(); }
            return true;
        }
    }
}
