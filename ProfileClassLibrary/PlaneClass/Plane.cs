using System;
using System.Data.SqlClient;

namespace ProfileClassLibrary.PlaneClass
{
    public class Plane
    {
        public Plane(int airlineId)
        {
            string imageName = default;
            DataBase data = new DataBase();
            string query = $"SELECT Company FROM Airline WHERE Id={airlineId};";
            SqlCommand command = new SqlCommand(query, data.GetConnection());
            data.OpenConnection();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        imageName = (string)reader.GetValue(0);
            }
            catch (Exception) { }
            if (imageName == "Белавиа")
                ImagePath = "https://airport.by/upload/images/4e3bdc6556844bde1c84c53fc65a8cc6.png";
            else if (imageName == "Аэрофлот")
                ImagePath = "https://airport.by/upload/images/3a4144634cb5678c4c6fb1aad958c551.png";
        }
        public string ImagePath { get; set; }
        public string FlightName { get; set; }
        public string Direction { get; set; }
        public string Gate { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Status { get; set; }
    }
}
