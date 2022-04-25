using System;
using System.Windows;
using System.Windows.Input;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;

namespace Курсовая.MainFrameForms.SityBusPages
{
    public partial class CountOfBusTickets : Window
    {
        private INavigation navigation;
        private IDataProcessing dataProcessing;
        private IRegComeIn regComeIn;
        public static bool CanRemoveFromBucket;

        public CountOfBusTickets(string buttonText)
        {
            InitializeComponent();

            navigation = new ProgrammNavigation();
            dataProcessing = new DataProcessing();
            regComeIn = new RegComeIn();

            Confirm.Content = buttonText;
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(CountTicket.Text)>0)
            {
                SityBusPage.CountOfTickets = Convert.ToInt32(CountTicket.Text);
                navigation.Cancellation(this);
            }
            else
                NotificationBox.Text = "Количество билет должно быть больше чем - 0";
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Cancellation_Click(object sender, RoutedEventArgs e)
        {
            SityBusPage.CountOfTickets = 0;
            navigation.Cancellation(this);
        }

        private void CountTicket_PreviewTextInput(object sender, TextCompositionEventArgs e)=>
            dataProcessing.NumberProcessing(sender,e);
        private void TextClear_GotFocus(object sender, RoutedEventArgs e) =>
            regComeIn.TextClear(sender, e);

    }
}
