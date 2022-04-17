using System.Collections.Generic;
using System.Windows.Controls;
using ProfileClassLibrary.BusClasses;


namespace Курсовая.MainFrameForms.SityBusPages
{
    public partial class BusNumbersPage : Page
    {
        private Frame BusSheduleFrame;
        private Border BorderBack;
        
        private string _city;

        public BusNumbersPage(Frame frame,Border BackBorder,string city)
        {
            InitializeComponent();
            DataContext = this;
            BusSheduleFrame = frame;
            BorderBack = BackBorder;
            _city = city;
            BusList = BusItems.GetBuses(_city);
        }

        public List<Bus> BusList { get; set; } 
       
        private void BusNumList_SelectionChanged(object sender, SelectionChangedEventArgs e)=>
            BusSheduleFrame.Navigate(new StationsPage(BusList[BusNaumList.SelectedIndex].Number, _city,BorderBack));
    }
}
