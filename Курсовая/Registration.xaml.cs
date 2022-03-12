using System;
using System.Collections.Generic;
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

namespace Курсовая
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
            reg.Visibility = Visibility.Visible;
            Button_reg.Foreground = Brushes.Blue;
            log.Visibility = Visibility.Hidden;
            Button_log.Foreground = Brushes.Black;
        }

        private void Cancellation_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainRoot.registration = null;
            //Application.Current.Shutdown();
        }

    }
}
