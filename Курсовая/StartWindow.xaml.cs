using System.Windows;
using System.Windows.Input;
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
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartFrame.Content = new MainRoot();
            //if (File.Exists(System.IO.Path.Combine(GetFolderPath(SpecialFolder.Windows), "Fonts", "SegoeIcons.ttf")))
            //{

            //}
            //else
            //{
            //    MessageBox.Show("нету");

            //    File.Copy("Resourse dictionary/MainFrameStyle/Segoe Fluent Icons.ttf", System.IO.Path.Combine(GetFolderPath(SpecialFolder.Windows),
            //            "Fonts", "Segoe Fluent Icons.ttf"));
            //    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");
            //    key.SetValue("Шрифт для иконок", "Segoe Fluent Icons.tff");
            //    key.Close();
            //}

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }
    }
}
