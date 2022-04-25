using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для BusThisStop.xaml
    /// </summary>
    public partial class BusThisStop : Page
    {
        private DataBase data;

        public BusThisStop(/*string busStop, string city*/)
        {
            string busStop = "Железнодорожный вокзал";
            string city = "Пинск";
            data = new DataBase();
            List<BusTimeTable> buses = GetBus(busStop, GetAllIdBus(city), city);
            BusList = buses;
        }
        public List<BusTimeTable> BusList { get; set; }

        private List<BusTimeTable> GetBus(string busStop, List<int> id, string sity)
        {
            List<BusTimeTable> busList = new List<BusTimeTable>();
            data.OpenConnection();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            foreach (var item in id)
            {
                string query = $"SELECT DISTINCT BStop FROM BusStop WHERE BStop ='{busStop}' AND BusId='{item}';";
                SqlCommand command = new SqlCommand(query, data.GetConnection());
                data.OpenConnection();
                try
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(table);
                    if (table.Rows.Count > 0)
                    {
                        query = $"SELECT BusName,FirstRoute FROM Bus WHERE PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City = '{sity}') AND Id ='{item}';";
                        command = new SqlCommand(query, data.GetConnection());
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                            while (reader.Read())
                                busList.Add(new BusTimeTable((string)reader.GetValue(0), (string)reader.GetValue(1)));
                        reader.Close();
                        table.Rows.Clear();
                    }
                }
                catch { }
            }
            data.CloseConnection();
            return busList;
        }
        private List<int> GetAllIdBus(string sity)
        {
            List<int> ids = new List<int>();
            string query = $"SELECT Id FROM Bus WHERE PublicBusCitiesId = (SELECT Id FROM PublicBusCities WHERE City = '{sity}');";
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        ids.Add(reader.GetInt32(0));
                reader.Close();
            }
            catch { }
            finally
            {
                command.Dispose();
            }
            return ids;
        }
    }
    public class BusTimeTable
    {
        public BusTimeTable(string nameBus, string route)
        {
            _nameBus = nameBus;
            _route = route;
        }
        private string _nameBus;
        public string NameBus { get => _nameBus; }

        private string _route;
        public string Route { get => _route; }
    }
}
