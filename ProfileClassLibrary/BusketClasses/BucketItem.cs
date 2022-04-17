using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileClassLibrary.BusketClasses
{
    public enum TransportType
    {
        Train,
        Plane,
        SityBus,
        Bus
    }
    public class BucketItem
    {
       
        public BucketItem(string direction, DateTime departureTime, DateTime departureDate, string transportNumber, int ticketNum, bool status, TransportType trType)
        {
            Direction = direction;
            DepartureTime = departureTime;
            DepartureDate = departureDate;
            TransportNumber = transportNumber;
            TicketNum = ticketNum;
            _purchaceStatus = status;
            _TransportType = trType;
        }
        public string TransportTypeIcon
        {
            get
            {
                if (_TransportType == TransportType.Train)
                {
                    return "&#xe7c0";
                }
                else if (_TransportType == TransportType.Plane)
                {
                    return "&#xE709;";
                }
                else if (_TransportType == TransportType.Bus)
                {
                    return "&#xeb47;";
                }
                else
                {
                    return "&#xe806;";
                }
            }
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
        public TransportType _TransportType;
        public string Direction { get; private set; }
        public DateTime DepartureTime { get; private set; }
        public DateTime DepartureDate { get; private set; }
        public string TransportNumber { get; private set; }
        public int TicketNum { get; private set; }
    }
}
