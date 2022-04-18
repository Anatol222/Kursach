using System.Collections.Generic;
using System.Windows.Controls;
using ProfileClassLibrary.BusClasses;


namespace Курсовая.MainFrameForms.SityBusPages
{
    public partial class BusNumbersPage : Page
    {
        private Frame BusSheduleFrame;
        private Border BorderBack;
        private Button ByTicket;
        private Button GoToBucket;

        private string _city;
        
        public BusNumbersPage(Frame frame,Border BackBorder, Button ByTicket, Button GoToBucket, string city)
        {
            InitializeComponent();
            DataContext = this;
            BusSheduleFrame = frame;
            BorderBack = BackBorder;
            this.ByTicket = ByTicket;
            this.GoToBucket = GoToBucket;
            _city = city;
            BusList = BusItems.GetBuses(_city);
        }

        public List<Bus> BusList { get; set; }

        private void BusNumberNav_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BusSheduleFrame.Navigate(new StationsPage( BusList[BusNaumList.SelectedIndex].Number, _city, BusSheduleFrame, ByTicket,GoToBucket,BorderBack));
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!BusSheduleFrame.NavigationService.CanGoBack)
            {
                BorderBack.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
