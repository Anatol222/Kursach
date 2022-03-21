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
    /// Логика взаимодействия для NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        private WorkWithInterface workWithInterface;
        private NotificationWindow NW;

        public NotificationWindow(string text)
        {
            InitializeComponent();

            workWithInterface = new WorkWithInterface();
            NW = this;

            NotificationBox.Text = text;
        }

        private void Cancellation_Click(object sender, RoutedEventArgs e)=>
            workWithInterface.Cancellation(sender, e, NW);
    }
}
