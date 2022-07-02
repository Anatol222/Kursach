using System.Threading;
using System.Windows;
using System.Windows.Input;
using Курсовая.Setting;
using static System.Environment;

namespace Курсовая
{
    public partial class StartWindow : Window
    {
        public static StartWindow startWindow;
        public StartWindow()
        {

            InitializeComponent();
            startWindow = this;
            GetDataForOptimization optimization = new GetDataForOptimization();
            Thread plane = new Thread(optimization.ChangingDatePlane);
            plane.Start();
            
            object locker = new object();
            lock (locker)
            {
                Thread train = new Thread(optimization.StartTrain);
                train.Start();
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)=>
            this.DragMove();

        private void Window_Loaded(object sender, RoutedEventArgs e)=>
            StartFrame.Content = new MainRoot();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Application.Current.MainWindow.WindowState = WindowState.Normal;
        }
    }
}
