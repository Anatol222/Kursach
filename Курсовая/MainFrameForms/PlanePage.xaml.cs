using ProfileClassLibrary.PlaneClass;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SqlClient;
using Курсовая.Setting;
using Курсовая.MainFrameForms.PlanePages;
using Курсовая.MainFrameForms.SityBusPages;
using Курсовая.ProgrammInterface;
using MaterialDesignThemes.Wpf;

namespace Курсовая.MainFrameForms
{
    public partial class PlanePage : Page
    {
        private DataBase data;
        private IDataProcessing dataProcessing;
        private INavigation navigation;
        private IDataBaseUserDataVerification userDataVerification;

        private delegate void InvoceWarningBox(string content, string buttonText);
        private event InvoceWarningBox Warning;

        private delegate void InvoceMessageBox(string messange);
        private event InvoceMessageBox Notification;

        public static string FlightClassString;
        public static int PeopleCount { get; set; }
        public static int AllLuggageCount { get; set; }
        public List<Plane> Planes { get; set; }

        public PlanePage()
        {
            InitializeComponent();
            DataContext = this;
            DayComboBox.SelectedIndex = 3;

            navigation = new ProgrammNavigation();
            dataProcessing = new DataProcessing();
            data = new DataBase();

            Notification += navigation.Display;
            
            Planes = GetDateFromBD();
            FillingChoiceCountry();

            PeopleCount = 1;
            ChoiceCountry.SelectedIndex = 0;
            DayComboBox.SelectedIndex = 2;

            userDataVerification = new UserDataVerification();
            Warning = userDataVerification.Display;
        }

        private void FillingChoiceCountry()
        {
            string query = "SELECT DISTINCT Direction FROM Plane";
            SqlCommand command = new SqlCommand(query, data.GetConnection());
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
            catch (Exception) { }
            finally { data.CloseConnection(); }
        }

        private List<Plane> GetDateFromBD()
        {
            List<Plane> planes = new List<Plane>();
            string query = "SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate,Id FROM Plane;";
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
            return planes;
        }
        private void FlightsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ByTicket.Visibility = Visibility.Visible;
            GoToBucket.Visibility = Visibility.Visible;
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
            flightSettingsWindow.ShowDialog();
            TextBlock PeopleTb = (TextBlock)FlightSettingsBtn.Template.FindName("People", FlightSettingsBtn);
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

        private void DirectionBox_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            dataProcessing.CompanyProcessing(sender, e);

        private void FlightBox_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            dataProcessing.NameProcessing(sender, e);

        private void TimeDeparture_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            dataProcessing.TimeProcessing(sender, e);

        private void Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{DirectionBox.Text}%'" +
                $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' {DayCombo()};";
            if (AirlineBox.Text!="")
                query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{DirectionBox.Text}%'" +
                $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' {DayCombo()} ;" +
                $"AND (AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%') OR AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%' ORDER BY Id DESC));";
            InvokeBD(query);
        }

        private void DirectionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DirectionBox.Text != ChoiceCountry.SelectedItem.ToString())
            {
                string query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{DirectionBox.Text}%'" +
                $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' {DayCombo()};";
                if (AirlineBox.Text != "")
                    query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{DirectionBox.Text}%'" +
                    $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' {DayCombo()}" +
                    $"AND (AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%') OR AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%' ORDER BY Id DESC));";
                InvokeBD(query);
                ChoiceCountry.SelectedIndex = 0;
            }
        }

        private void AirlineBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           string query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{DirectionBox.Text}%'" +
                $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' {DayCombo()} " +
                $"AND (AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%') OR AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%' ORDER BY Id DESC));";
            InvokeBD(query);
        }

        private void ChoiceCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{ChoiceCountry.SelectedItem.ToString()}%'" +
                $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' {DayCombo()};";
            if (AirlineBox.Text != "")
            {
                query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{ChoiceCountry.SelectedItem.ToString()}%'" +
                  $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' {DayCombo()} " +
                  $"AND (AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%') OR AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%' ORDER BY Id DESC));";
            }
            if (ChoiceCountry.SelectedItem.ToString() == "Любая")
            {
                query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE " +
                $" Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' {DayCombo()};";
                if (AirlineBox.Text != "")
                {
                    query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE " +
                    $" Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' {DayCombo()} " +
                    $"AND (AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%') OR AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%' ORDER BY Id DESC));";
                }
                DirectionBox.Text = default;
            }
            InvokeBD(query);
            if (ChoiceCountry.SelectedItem.ToString() != "Любая")
                DirectionBox.Text = ChoiceCountry.SelectedItem.ToString();
        }

        private void GoToBucket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = $"INSERT INTO ShoppingBasket(IdPersonalLoginData,TicketWhichTransport,RouteTicket,DepartureTime,DepartureDate,TicketStatus,CountTickets,TransportName,TypeService)" +
                    $"VALUES((SELECT Id FROM PersonalLoginData WHERE Email = '{MainFrame.user.Email}'), '1', '{Planes[FlightsListBox.SelectedIndex].Direction}'," +
                    $" '{Planes[FlightsListBox.SelectedIndex].DepartureTime.ToString(@"HH\:mm")}', '{Planes[FlightsListBox.SelectedIndex].DepartureDate.Day}-{Planes[FlightsListBox.SelectedIndex].DepartureDate.Month}-{Planes[FlightsListBox.SelectedIndex].DepartureDate.Year}', " +
                    $"0, {PeopleCount}, '{Planes[FlightsListBox.SelectedIndex].FlightName}', '{(string)FlightClass.Content}'); ";
                if (AddTicketIntoBD(query))
                {
                    Notification?.Invoke("Билет добавлен в корзину, не забудьте оплатить");
                    MainFrame.BasketItemsCount += 1;
                    MainFrame.basketButton.GetBindingExpression(TagProperty).UpdateTarget();
                    Badged badged = (Badged)MainFrame.basketButton.Template.FindName("basketItemsCount", MainFrame.basketButton);
                    badged.GetBindingExpression(Badged.BadgeProperty).UpdateTarget();
                }
                else
                    Notification?.Invoke("Билетов в таком количестве нет");
            }
            catch (Exception) { }
        }
        
        private void ByTicket_Click(object sender, RoutedEventArgs e)
        {
            Warning?.Invoke("Вы уверены, что хотите приобрести билет?", "Купить");
            if (SityBusPage.ConfirmBuyTicket)
            {
                string query = $"INSERT INTO ShoppingBasket(IdPersonalLoginData,TicketWhichTransport,RouteTicket,DepartureTime,DepartureDate,TicketStatus,CountTickets,TransportName,TypeService)" +
                $"VALUES((SELECT Id FROM PersonalLoginData WHERE Email = '{MainFrame.user.Email}'), '1', '{Planes[FlightsListBox.SelectedIndex].Direction}'," +
                $" '{Planes[FlightsListBox.SelectedIndex].DepartureTime.ToString(@"HH\:mm")}', '{Planes[FlightsListBox.SelectedIndex].DepartureDate.Day}-{Planes[FlightsListBox.SelectedIndex].DepartureDate.Month}-{Planes[FlightsListBox.SelectedIndex].DepartureDate.Year}', " +
                $"1, {PeopleCount}, '{Planes[FlightsListBox.SelectedIndex].FlightName}', '{(string)FlightClass.Content}'); ";
                if (AddTicketIntoBD(query))
                {
                    Notification?.Invoke("Спасибо за покупку. Билет у вас в корзине");
                    MainFrame.BasketItemsCount += 1;
                    MainFrame.basketButton.GetBindingExpression(TagProperty).UpdateTarget();
                    Badged badged = (Badged)MainFrame.basketButton.Template.FindName("basketItemsCount", MainFrame.basketButton);
                    badged.GetBindingExpression(Badged.BadgeProperty).UpdateTarget();
                }
                else
                    Notification?.Invoke("Билетов в таком количестве нет");
            }
            SityBusPage.ConfirmBuyTicket = false;
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

        private string DayCombo()
        {
            DateTime date = DateTime.Now.AddDays(1);
            string s = DayComboBox.Text.ToString();
            if (DayComboBox.Text.ToString() == "Завтра")
                return $" AND DepartureDate = '{((DateTime)date).Day}-{((DateTime)date).Month}-{((DateTime)date).Year}' ";
            else if (DayComboBox.Text.ToString() == "Сегодня")
                return $" AND DepartureDate = '{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}' ";
            else
                return "";
        }

        private void DayComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string dateSelect = default;
            DateTime date = DateTime.Now.AddDays(1);
            if (DayComboBox.SelectedIndex == 0)
                dateSelect = $"AND DepartureDate = '{((DateTime)date).Day}-{((DateTime)date).Month}-{((DateTime)date).Year}' ";
            else if (DayComboBox.SelectedIndex == 1)
                dateSelect = $" AND DepartureDate = '{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}' ";
            else
                dateSelect = "";
            string query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{DirectionBox.Text}%'" +
                $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' {dateSelect};";
            if (AirlineBox.Text != "")
                query = $"SELECT AirlineId,Flight,Direction,Landing,DepartureTime,StatusPlane,DepartureDate  FROM Plane WHERE Direction LIKE '%{DirectionBox.Text}%'" +
                $"AND Flight LIKE '%{FlightBox.Text}%' AND DepartureTime LIKE '%{TimeDeparture.Text}%' {dateSelect}'" +
                $"AND (AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%') OR AirlineId = (SELECT TOP 1 Id FROM Airline WHERE Company LIKE '%{AirlineBox.Text}%' ORDER BY Id DESC));";
            InvokeBD(query);
        }
    }
}
