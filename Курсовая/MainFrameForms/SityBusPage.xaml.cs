using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Курсовая.MainFrameForms.SityBusPages;
using System.Data.SqlClient;

namespace Курсовая.MainFrameForms
{

    public partial class SityBusPage : Page
    {
        private DataBase data;
        public SityBusPage()
        {
            InitializeComponent();
            data = new DataBase();
            sities = new List<string>();
            FillInCities();
            DataContext = this;

        }

        public List<string> sities { get; set; }

        private void SityComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            BusSheduleFrame.Navigate(new BusNumbersPage(BusSheduleFrame, BackBorder, ByTicket, GoToBucket, SityComboBox.SelectedItem.ToString()));
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BusSheduleFrame.NavigationService.CanGoBack)
            {
                BusSheduleFrame.NavigationService.GoBack();
            }
            
        }
        private void FillInCities()
        {
            string query = "SELECT City FROM PublicBusCities;";
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                    sities.Add((string)reader.GetValue(0));
            reader.Close();
            data.CloseConnection();
        }
    }
}
