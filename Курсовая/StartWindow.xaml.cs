using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using static System.Environment;

namespace Курсовая
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
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
            if (File.Exists(System.IO.Path.Combine(GetFolderPath(SpecialFolder.Windows), "Fonts", "SegoeIcons.ttf")))
            {

            }
            else
            {
                MessageBox.Show("нету");

                File.Copy("Resourse dictionary/MainFrameStyle/Segoe Fluent Icons.ttf", System.IO.Path.Combine(GetFolderPath(SpecialFolder.Windows),
                        "Fonts", "Segoe Fluent Icons.ttf"));
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");
                key.SetValue("Шрифт для иконок", "Segoe Fluent Icons.tff");
                key.Close();
            }

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
