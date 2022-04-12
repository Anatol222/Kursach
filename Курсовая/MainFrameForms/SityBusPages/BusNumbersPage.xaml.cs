using ProfileClassLibrary.BusClasses;
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
    /// Логика взаимодействия для BusNumbersPage.xaml
    /// </summary>
    public partial class BusNumbersPage : Page
    {
        private Frame BusSheduleFrame;
        public BusNumbersPage(Frame frame)
        {
            InitializeComponent();
            DataContext = this;
            BusSheduleFrame = frame;    
        }
        public List<Bus> BusList { get; set; } = BusItems.GetBuses();
        public class BusItems
        {
            public static List<Bus> GetBuses()
            {
                return new List<Bus>()
                {
                    new Bus(){Number = "1"},
                    new Bus(){Number = "2"},
                    new Bus(){Number = "3"}
                };
            }
        }

        public class Bus
        {
            public string Number { get; set; }
            public override string ToString()
            {
                return Number.ToString();
            }
        }
        private void BusNumList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BusSheduleFrame.Navigate(new StationsPage("1", "fwffe"));
        }
    }
}
