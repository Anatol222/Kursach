using System.Collections.Generic;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using ProfileClassLibrary.BusClasses;
using System;

namespace Курсовая.MainFrameForms.SityBusPages
{
    public partial class BusesOnStationPage : Page
    {
        private DataBase data;

        //private Frame BusSheduleFrame;
        //private Button ByTicket;
        //private Button GoToBucket;
        public List<BusTimeTable> BusList { get; set; }
        public BusesOnStationPage(Frame frame, Button ByTicket, Button GoToBucket, string busStop, string city)
        {
            InitializeComponent();
            DataContext = this;
            data = new DataBase();
            BusList = GetBus(busStop, GetAllIdBus(city), city);
            InfoBus.Text = $"Расписание транспорта на остановке {busStop} - {city}";
        }
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
                catch (Exception){ }
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
            catch (Exception) { }
            finally
            {
                command.Dispose();
                data.CloseConnection();
            }
            return ids;
        }
    }
}
