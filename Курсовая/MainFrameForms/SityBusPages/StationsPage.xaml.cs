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
    /// Логика взаимодействия для StationsPage.xaml
    /// </summary>
    public partial class StationsPage : Page
    {
        public StationsPage(string busNum, string sity,Border BackBorder)
        {
            InitializeComponent();
            DataContext = this;
            BackBorder.Visibility = Visibility.Visible;
            //Сделать запрос для вытягивания направлений 
            string Fdr = "";
            string Sdr ;
            Direction Sdirection;
            Direction Fdirection = new Direction(Fdr);
            try
            {
                Sdr = "";
                Sdirection = new Direction(Sdr);
                Sdirection.busStations = Direction.GetStations(/*busNum, Sdr*/);

            }
            catch (Exception)
            {

                throw;
            }
            Fdirection.busStations = Direction.GetStations(/*busNum, Fdr*/);
        }
        public List<Direction> directions { get; set; } = new List<Direction>() { new Direction("1323-2134grgggggggggggggggggreg") { busStations = Direction.GetStations() }, new Direction("1323-2134rgewgerwgewgewrg") { busStations = Direction.GetStations() } };
        private void StationsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class Direction
    {
        public Direction(string name)
        {
            DrName = name;
        }
        private List<Busstation> _busstations;
        public string DrName { get; set; }
        public List<Busstation> busStations { get { return _busstations; } set { _busstations = value; } }
        public static List<Busstation> GetStations(/*string busNum, string DrName*/)
        {
            //сделать вытягивание
            return new List<Busstation>() { new Busstation() { StName = "123" }, new Busstation() { StName = "123" } };
        }

    }

    public class Busstation
    {
        public string StName { get; set; }

    }
}
