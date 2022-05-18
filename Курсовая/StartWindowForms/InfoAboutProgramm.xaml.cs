using System.Windows;
using System.Windows.Input;

namespace Курсовая
{
    public partial class InfoAboutProgramm : Window
    {
        private TextInterface workWithInterface;

        public InfoAboutProgramm()
        {
            InitializeComponent();

            workWithInterface = new TextInterface();

            InfoProg.Text = workWithInterface.AboutAppHorizon;
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainRoot.infoWindow = null;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();
    }
}
