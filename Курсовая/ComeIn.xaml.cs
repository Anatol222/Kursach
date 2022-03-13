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
    /// Логика взаимодействия для ComeIn.xaml
    /// </summary>
    public partial class ComeIn : Window
    {
        private WorkWithInterface workWithInterface;
        
        public ComeIn()
        {
            InitializeComponent();

            reg.Visibility = Visibility.Hidden;
            Button_reg.Foreground = Brushes.Black;

            log.Visibility = Visibility.Visible;
            Button_log.Foreground = Brushes.Blue;
            
            workWithInterface = new WorkWithInterface();
        }

        private void Cancellation_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainRoot.windowEntrance = null;
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainRoot.windowEntrance = new Registration();
            MainRoot.windowEntrance.Show();

        }
    }
}
