using ProfileClassLibrary.PlaneClass;
using ProfileClassLibrary.TrainClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;

namespace Курсовая.Setting
{
    public class GetDataForOptimization
    {
        DataBase data = new DataBase();
        public static List<Train> StartTrains { get; private set; }

        public void StartTrain()
        {
            DataBase db = new DataBase();
            StartTrains = new List<Train>();
            string query = "SELECT NameTrain,Direction,Departure,Arrival,TrainType,Id FROM Train";
            SqlCommand command = new SqlCommand(query, db.GetConnection());
            db.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        try
                        {
                            string[] city = ((string)reader.GetValue(1)).Replace(" - ", "!").Split('!');
                            DateTime departure = Convert.ToDateTime(Convert.ToString(reader.GetValue(2)).Substring(0, 5));
                            DateTime arrival = Convert.ToDateTime(Convert.ToString(reader.GetValue(3)).Substring(0, 5));
                            TimeSpan TimeAllWay;
                            if (arrival.Hour < departure.Hour)
                                TimeAllWay =(new TimeSpan(23,59,0) -(departure - arrival));
                            else
                                TimeAllWay = arrival - departure;
                            StartTrains.Add(new Train((string)reader.GetValue(4), (string)reader.GetValue(0), (string)reader.GetValue(1), departure, city[0], arrival, city[1], TimeAllWay, Convert.ToInt32(reader.GetValue(5))));
                        }
                        catch (Exception) { }
                    reader.Close();
                }
            }
            catch (Exception) { }
            finally { db.CloseConnection(); }
        }

        public void ChangingDatePlane()
        {
            DataBase dataBase = new DataBase();
            QueryBD($"DELETE FROM Plane WHERE ((DepartureTime< '{DateTime.Now.ToString(@"HH\:mm")}' AND DepartureDate <= '{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}') OR DepartureDate < '{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}');");
            string query = $"SELECT * FROM Plane WHERE Landing IS NOT NULL;";
            SqlCommand command = new SqlCommand(query, dataBase.GetConnection());
            dataBase.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                    InsertDataInBD(ReadingPlaneFromNet());
                QueryBD($"UPDATE Plane SET StatusPlane = 'Регистрация' WHERE DepartureTime < '{DateTime.Now.AddHours(1).AddMinutes(30).ToString(@"HH\:mm")}' " +
                        $"AND Landing IS NOT NULL;");
                QueryBD($"UPDATE Plane SET StatusPlane = 'Посадка' WHERE DepartureTime < '{DateTime.Now.AddMinutes(45).ToString(@"HH\:mm")}' AND Landing IS NOT NULL;");
            }
            catch (Exception) { }
            finally { dataBase.CloseConnection(); }
        }

        private void QueryBD(string query)
        {
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try { command.ExecuteNonQuery(); }
            catch (Exception) { }
            finally { data.CloseConnection(); }
        }

        private List<Flight> ReadingPlaneFromNet()
        {
            List<Flight> flights = new List<Flight>();
            try
            {
                byte[] bytes = new byte[1000];
                HttpWebRequest rq = (HttpWebRequest)WebRequest.Create("https://airport.by/ru/flights/departure");
                rq.Method = "POST";
                rq.ContentLength = bytes.Length;
                rq.ContentType = "text/json; encoding='utf-8'";
                using (Stream str = rq.GetRequestStream())
                    str.Write(bytes, 0, bytes.Length);
                HttpWebResponse httpWebResponse = (HttpWebResponse)rq.GetResponse();
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    DataContractJsonSerializer dataContractJson = new DataContractJsonSerializer(typeof(List<Flight>));
                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                        flights = (List<Flight>)dataContractJson.ReadObject(responseStream);
                }
                httpWebResponse.Close();
            }
            catch (Exception) { }
            return flights;
        }

        private void InsertDataInBD(List<Flight> flights)
        {
            data.OpenConnection();
            Random rnd = new Random();
            string query = default;
            SqlCommand command = default;
            foreach (var item in flights)
            {
                bool access = default;
                query = $"INSERT INTO Airline(Company) VALUES('{item.airline.title}'); ";
                command = new SqlCommand(query, data.GetConnection());
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable dataTable = new DataTable();
                try
                {
                    try
                    {
                        if (command.ExecuteNonQuery() > 0)
                            access = true;
                    }
                    catch
                    {
                        query = $"SELECT * FROM Airline WHERE Company ='{item.airline.title}';";
                        command = new SqlCommand(query, data.GetConnection());
                        adapter.SelectCommand = command;
                        adapter.Fill(dataTable);
                        if (dataTable.Rows.Count > 0)
                            access = true;
                    }
                    if (access && item.status.title != "Вылетел" && item.status.title != "Отменен")
                    {
                        if (item.numbers_gate.Length != 0)
                        {
                            query = $"SELECT Id FROM Plane WHERE Flight ='{item.flight}' AND Direction ='{item.airport.title}' AND DepartureTime ='{Convert.ToDateTime(item.plan).ToString(@"HH\:mm")}' " +
                                $"AND DepartureDate = '{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}';";
                            command = new SqlCommand(query, data.GetConnection());
                            try
                            {
                                if (command.ExecuteNonQuery() > 0)
                                    query = $"UPDATE Plane SET Landing = '{item.numbers_gate[0]}' WHERE Flight = '{item.flight}'" +
                                  $" AND Direction ='{item.airport.title}' AND DepartureTime ='{Convert.ToDateTime(item.plan).ToString(@"HH\:mm")}' " +
                                  $"AND DepartureDate ='{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}';";
                                else
                                    query = $"INSERT INTO Plane(AirlineId,Flight,Direction,DepartureTime,StatusPlane,DepartureDate,Landing) " +
                                         $"VALUES((SELECT ID FROM Airline WHERE Company = '{item.airline.title}'),'{item.flight}','{item.airport.title}'," +
                                         $"'{Convert.ToDateTime(item.plan).ToString(@"HH\:mm")}','{item.status.title}'," +
                                         $"'{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}', '{item.numbers_gate[0]}'); ";
                            }
                            catch (Exception) { }
                            access = false;
                        }
                        else if (item.numbers_gate.Length == 0)
                        {
                            DateTime date = DateTime.Now.AddDays(1);
                            query = $"INSERT INTO Plane(AirlineId,Flight,Direction,DepartureTime,StatusPlane,DepartureDate) " +
                                         $"VALUES((SELECT ID FROM Airline WHERE Company = '{item.airline.title}'),'{item.flight}','{item.airport.title}'," +
                                         $"'{Convert.ToDateTime(item.plan).ToString(@"HH\:mm")}','{item.status.title}'," +
                                         $"'{((DateTime)date).Month}-{((DateTime)date).Day}-{((DateTime)date).Year}'); ";
                            access = true;
                        }
                        command = new SqlCommand(query, data.GetConnection());
                        if (command.ExecuteNonQuery() > 0 && access)
                        {
                            try
                            {
                                query = $"INSERT INTO NumberSeats(PlaneId,BusinessClass,EconomyClass,PremiumEconomyClass,FirstClass) " +
                                    $"VALUES((SELECT ID FROM Plane WHERE Flight = '{item.flight}' AND StatusPlane='{item.status.title}' " +
                                    $"AND DepartureTime ='{Convert.ToDateTime(item.plan).ToString(@"HH\:mm")}'  AND Landing IS NULL AND Direction='{item.airport.title}')," +
                                    $"{rnd.Next(0, 30)},{rnd.Next(0, 80)},{rnd.Next(0, 20)},{rnd.Next(0, 50)}); ";
                                command = new SqlCommand(query, data.GetConnection());
                                command.ExecuteNonQuery();
                            }
                            catch (Exception) { }
                        }
                    }
                }
                catch (Exception) { }
            }
            query = $"DELETE FROM Plane WHERE Landing IS NULL AND DepartureDate = '{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}';";
            command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try { command.ExecuteNonQuery(); }
            catch (Exception) { }
            data.CloseConnection();
        }
    }
}
