using System.Windows;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;

namespace Курсовая
{
    public partial class NotificationWindow : Window
    {
        private NotificationWindow NW;

        private INavigation navigation;

        public NotificationWindow(string text)
        {
            InitializeComponent();

            NW = this;
            navigation = new ProgrammNavigation();

            NotificationBox.Text = text;
        }

        private void Cancellation_Click(object sender, RoutedEventArgs e)=>
            navigation.Cancellation(NW);
    }
}
