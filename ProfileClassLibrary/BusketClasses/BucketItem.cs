using System;

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
        public bool _purchaceStatus;
        public TransportType _TransportType;
        public string Direction { get; private set; }
        public DateTime DepartureTime { get; private set; }
        public DateTime DepartureDate { get; private set; }
        public string TransportNumber { get; private set; }
        public int TicketNum { get; private set; }
        public int Id { get; private set; }
        public string TypeService { get; private set; }
        public int TicketWhichTransport { get; private set; }

        public BucketItem(string direction, DateTime departureTime, DateTime departureDate, string transportNumber, int ticketNum, bool status, TransportType trType, string typeService, int id)
        {
            Direction = direction;
            DepartureTime = departureTime;
            DepartureDate = departureDate;
            TransportNumber = transportNumber;
            TicketNum = ticketNum;
            _purchaceStatus = status;
            _TransportType = trType;
            TypeService = typeService;
            Id = id;
            TicketWhichTransport = (int)trType;
        }

        public string TransportTypeIcon
        {
            get
            {
                if (_TransportType == TransportType.Train)
                    return "Train";
                else if (_TransportType == TransportType.Plane)
                    return "Plane";
                else if (_TransportType == TransportType.Bus)
                    return "Bus";
                else
                    return "Bus";
            }
        }
        public string PurchaceStatusIcon
        {
            get
            {
                if (_purchaceStatus == true)
                    return "/./Images/BucketPageIcons/icons8-оплачено-100.png";
                else
                    return "/./Images/BucketPageIcons/Оплатить.png";
            }
        }
        public string CanReturnMoneyIcon
        {
            get
            {
                if (_purchaceStatus == true)
                    return "/./Images/BucketPageIcons/вернуть-покупку.png";
                else
                    return "/./Images/BucketPageIcons/Удалить из корзыны.png";
            }
        }
    }
}
