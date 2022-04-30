using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProfileClassLibrary.BusketClasses
{
    public class Bucket
    {
        public List<BucketItem> GetItems(string email,SqlConnection sqlConnection)
        {
            List<BucketItem> items = new List<BucketItem>();
            string query = $"SELECT TicketWhichTransport,RouteTicket,DepartureTime,DepartureDate,TicketStatus,CountTickets,TransportName,TypeService, Id FROM ShoppingBasket " +
                $"WHERE IdPersonalLoginData = (SELECT Id FROM PersonalLoginData WHERE Email = '{email}');";
            DateTime dateTime=default, date = default;
            bool statusTicket=default;
            SqlCommand command = new SqlCommand(query,sqlConnection);
            sqlConnection.Open();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        
                        try {
                            string[] vs = (Convert.ToString(reader.GetValue(2))).Split(':');
                            string s = $"{vs[0]}:{vs[1]}";
                            dateTime = Convert.ToDateTime(s); 
                        }
                        catch { dateTime = DateTime.Now; }
                        try { date = Convert.ToDateTime(reader.GetValue(3)); }
                        catch { date = DateTime.Now; }
                        if (Convert.ToInt32(reader.GetValue(4))==0)
                            statusTicket = false;
                        else
                            statusTicket = true;
                        items.Add(new BucketItem((string)reader.GetValue(1), dateTime, date,(string)reader.GetValue(6),Convert.ToInt32(reader.GetValue(5)), statusTicket, (TransportType)Convert.ToInt32(reader.GetValue(0)),(string)reader.GetValue(7),Convert.ToInt32(reader.GetValue(8))));
                    }
                }
            }
            catch (Exception) { }
            finally { sqlConnection.Close(); }
            return items;
        }
    }
}
