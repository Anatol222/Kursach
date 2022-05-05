
using System.Windows.Controls;

namespace Курсовая.MainFrameForms
{

    public partial class BusPage : Page
    {
        public BusPage()
        {
            InitializeComponent();
            string query = "SELECT District FROM RegionalBusDistrict;";
            BusSheduleFrame.Navigate(new SityBusPage(query,"регион"));
        }
    }
}
