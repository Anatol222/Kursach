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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Курсовая.MainFrameForms.SityBusPages
{
    /// <summary>
    /// Логика взаимодействия для BusesOnStationPage.xaml
    /// </summary>
    public partial class BusesOnStationPage : Page
    {
        private Frame BusSheduleFrame;
        private Button ByTicket;
        private Button GoToBucket;
        public BusesOnStationPage(Frame frame, Button ByTicket, Button GoToBucket)
        {
            InitializeComponent();
        }
    }
}
