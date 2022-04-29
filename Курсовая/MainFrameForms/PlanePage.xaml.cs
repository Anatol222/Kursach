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
        
        public PlanePage()
        {
            InitializeComponent();
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

        private void FlightClass_Click(object sender, RoutedEventArgs e)
        {
            FlightClassString = (string)FlightClass.Content;
            ClassChoiceWindow classChoiceWindow = new ClassChoiceWindow();
            classChoiceWindow.Left = PointToScreen(Mouse.GetPosition(this)).X-Mouse.GetPosition(this).X;
            classChoiceWindow.Top = PointToScreen(Mouse.GetPosition(this)).Y - Mouse.GetPosition(this).Y;
            
            classChoiceWindow.ShowDialog();

            FlightClass.Content = FlightClassString;
        }

        private void FlightSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            FlightSettingsWindow flightSettingsWindow = new FlightSettingsWindow();
            flightSettingsWindow.Show();
        }
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
