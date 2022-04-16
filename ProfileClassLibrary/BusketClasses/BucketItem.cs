using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileClassLibrary.BusketClasses
{
    public class BucketItem
    {
        public BucketItem(string direction, DateTime departureTime, DateTime departureDate, string transportNumber, int ticketNum, bool status)
        {
            Direction = direction;
            DepartureTime = departureTime;
            DepartureDate = departureDate;
            TransportNumber = transportNumber;
            TicketNum = ticketNum;
            _purchaceStatus = status;
        }
        public string PurchaceStatusIcon
        {
            get
            {
                if (_purchaceStatus == true)
                {
                    return "/./Images/BucketPageIcons/icons8-оплачено-100.png";
                }
                else
                {
                    return "/./Images/BucketPageIcons/Оплатить.png";
                }
            }
        }
        public string CanReturnMoneyIcon
        {
            get
            {
                if (_purchaceStatus == true)
                {
                    return "/./Images/BucketPageIcons/вернуть-покупку.png";
                }
                else
                {
                    return "/./Images/BucketPageIcons/Удалить из корзыны.png";
                }
            }
        }
        public bool _purchaceStatus;
        public string Direction { get; private set; }
        public DateTime DepartureTime { get; private set; }
        public DateTime DepartureDate { get; private set; }
        public string TransportNumber { get; private set; }
        public int TicketNum { get; private set; }
    }
}
