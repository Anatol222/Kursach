using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Курсовая.MainFrameForms.SityBusPages;
using System.Data.SqlClient;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;
using System;

namespace Курсовая.MainFrameForms
{
    public partial class SityBusPage : Page
    {
        private DataBase data;
        private CountOfBusTickets countOfBusTickets;

        private INavigation navigation;
        private IDataBaseUserDataVerification userDataVerification;

        private delegate void InvoceMessageBox(string messange);
        private event InvoceMessageBox Notification;

        private delegate void InvoceWarningBox(string content, string buttonText);
        private event InvoceWarningBox Warning;

        private delegate void InvoceCountTicketsBox(string buttonContent);
        private event InvoceCountTicketsBox TicketEvent;

        public static bool ConfirmBuyTicket {get;set;}
        public static int CountOfTickets { get;set;}
        public SityBusPage()
        {
            InitializeComponent();
            data = new DataBase();
            navigation = new ProgrammNavigation();
            userDataVerification = new UserDataVerification();
            Notification = navigation.Display;
            Warning = userDataVerification.Display;
            TicketEvent = Display;
            sities = new List<string>();
            FillInCities();
            DataContext = this;

        }

        public List<string> sities { get; set; }

        private void SityComboBox_SelectionChanged(object sender, RoutedEventArgs e)=>
            BusSheduleFrame.Navigate(new BusNumbersPage(BusSheduleFrame, BackBorder, ByTicket, GoToBucket, SityComboBox.SelectedItem.ToString()));

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BusSheduleFrame.NavigationService.CanGoBack)
                BusSheduleFrame.NavigationService.GoBack();
        }
        private async void FillInCities()
        {
            string query = "SELECT City FROM PublicBusCities;";
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                    while (reader.Read())
                        sities.Add((string)reader.GetValue(0));
                reader.Close();
            }
            catch (Exception) { }
            finally { data.CloseConnection(); }
        }

        private void GoToBucket_Click(object sender, RoutedEventArgs e)
        {
            TicketEvent?.Invoke("Добавить");
            if (CountOfTickets>0)
            {
                string query = "INSERT INTO ShoppingBasket (IdPersonalLoginData,TicketWhichTransport,RouteTicket,TicketStatus,CountTickets,TransportName) " +
                $"VALUES((SELECT Id FROM PersonalLoginData WHERE Email = '{MainFrame.user.Email}'),2,'{BusTimePage.aboutBus.Route}',0,{CountOfTickets},'{BusTimePage.aboutBus.BusName}');";
                if (AddTicketIntoBD(query))
                    Notification?.Invoke("Билет добавлен в корзину, не забудьте оплатить");
                else
                    Notification?.Invoke("Возникла ошибка при добавлении билета");
            }
            CountOfTickets = 0;
        }

        private void ByTicket_Click(object sender, RoutedEventArgs e)
        {
            TicketEvent?.Invoke("Купить");
            if (CountOfTickets > 0)
            {
                Warning?.Invoke("Вы уверены, что хотите приобрести билет?", "Купить");
                if (ConfirmBuyTicket)
                {
                    string query = "INSERT INTO ShoppingBasket (IdPersonalLoginData,TicketWhichTransport,RouteTicket,TicketStatus,CountTickets,TransportName) " +
                     $"VALUES((SELECT Id FROM PersonalLoginData WHERE Email = '{MainFrame.user.Email}'),2,'{BusTimePage.aboutBus.Route}',1,{CountOfTickets},'{BusTimePage.aboutBus.BusName}');";
                    if (AddTicketIntoBD(query))
                        Notification?.Invoke("Спасибо за покупку. Билет у вас в корзине");
                    else
                        Notification?.Invoke("Возникла ошибка при покупке билета");
                }
                ConfirmBuyTicket = false;
            }
            CountOfTickets = 0;
        }
        private bool AddTicketIntoBD(string query)
        {
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try { command.ExecuteNonQuery(); }
            catch { return false; }
            finally { data.CloseConnection(); }
            return true;
        }
        public void Display(string buttonContent)
        {
            if (countOfBusTickets != null)
                countOfBusTickets.Close();
            countOfBusTickets = new CountOfBusTickets(buttonContent);
            countOfBusTickets.ShowDialog();
        }
    }
}
