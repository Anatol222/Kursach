using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        private void BusNumberNav_Click(object sender, System.Windows.RoutedEventArgs e)=>
            BusSheduleFrame.Navigate(new StationsPage( BusList[BusNaumList.SelectedIndex].Number, _city, BusSheduleFrame, ByTicket,GoToBucket,BorderBack));

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!BusSheduleFrame.NavigationService.CanGoBack)
                BorderBack.Visibility = System.Windows.Visibility.Hidden;
        }
        private void BusNaumList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (null == listBox)
                return;

            var point = e.GetPosition((UIElement)sender);

            VisualTreeHelper.HitTest(listBox, null, (hitTestResult) =>
            {
                var uiElement = hitTestResult.VisualHit as UIElement;

                while (null != uiElement)
                {
                    var listBoxItem = uiElement as ListBoxItem;
                    if (null != listBoxItem)
                    {
                        listBoxItem.IsSelected = true;
                        return HitTestResultBehavior.Stop;
                    }

                    uiElement = VisualTreeHelper.GetParent(uiElement) as UIElement;
                }

                return HitTestResultBehavior.Continue;
            }, new PointHitTestParameters(point));
        }
    }
}
