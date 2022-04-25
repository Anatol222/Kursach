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

namespace Курсовая.MainFrameForms.SityBusPages
{
    /// <summary>
    /// Логика взаимодействия для BusTicketOrderWindow.xaml
    /// </summary>
    public partial class BusTicketOrderWindow : Window
    {
        public BusTicketOrderWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        public int KidCounter { get; set; }
        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            KidsCounter.Text = (int.Parse(KidsCounter.Text) + 1).ToString();
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            if ((int.Parse(KidsCounter.Text) - 1)>=0)
            {
               KidsCounter.Text = (int.Parse(KidsCounter.Text) - 1).ToString();
            }
        }

        private void KidsCounter_TextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
