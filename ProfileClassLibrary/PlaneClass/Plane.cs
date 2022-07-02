using System;
using System.Data.SqlClient;

namespace ProfileClassLibrary.PlaneClass
{
    public class Plane
    {
        public string ImagePath { get; set; }
        public string FlightName { get; set; }
        public string Direction { get; set; }
        public string Gate { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Status { get; set; }

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
                ImagePath = @"\Images\PlaneIcons\Белавиа.png";
            else if (imageName == "Аэрофлот")
                ImagePath = @"\Images\PlaneIcons\Аэрофлот.png";
            else if (imageName == "Uzbekistan Airways")
                ImagePath = @"\Images\PlaneIcons\УзбекисанАвиа.png";
        }
    }
}
