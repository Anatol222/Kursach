using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using ProfileClassLibrary.BusClasses;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;

namespace Курсовая.MainFrameForms.SityBusPages
{
    public partial class BusNumbersPage : Page
    {
        private Frame BusSheduleFrame;
        private Border BorderBack;
        private Button ByTicket;
        private Button GoToBucket;

        private IWorkWithBusList workWithBusList;

        public List<Bus> BusList { get; set; }
        private readonly string _city;
        
        public BusNumbersPage(Frame frame,Border BackBorder, Button ByTicket, Button GoToBucket, string city)
        {
            InitializeComponent();
            workWithBusList = new WorkWithBusList();
            DataContext = this;
            BusSheduleFrame = frame;
            BorderBack = BackBorder;
            this.ByTicket = ByTicket;
            this.GoToBucket = GoToBucket;
            _city = city;
            if (SityBusPage.BusType == default)
                BusList = BusItems.GetBuses($"SELECT BusName FROM Bus WHERE PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City='{_city}');");
            else
                BusList = BusItems.GetBuses($"SELECT BusName FROM RegionalBus WHERE RegionalBusDistrictId = (SELECT Id FROM RegionalBusDistrict WHERE District ='{_city}');");
        }

        private void BusNumberNav_Click(object sender, System.Windows.RoutedEventArgs e)=>
            BusSheduleFrame.Navigate(new StationsPage( BusList[BusNaumList.SelectedIndex].Number, _city, BusSheduleFrame, ByTicket,GoToBucket,BorderBack));

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!BusSheduleFrame.NavigationService.CanGoBack)
                BorderBack.Visibility = System.Windows.Visibility.Hidden;
        }

        private void BusNaumList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)=>
            workWithBusList.BucketListBoxFirst(sender,e);
    }
}
