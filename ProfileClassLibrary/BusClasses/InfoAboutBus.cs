namespace ProfileClassLibrary.BusClasses
{
    public class InfoAboutBus
    {
        private string _route;
        public string Route { get => _route; }

        private string _busName;
        public string BusName { get => _busName; }

        private string _busStop;
        public string BusStop { get => _busStop; }

        private string _busCity;
        public string BusCity { get => _busCity; }

        private string _busMovement;
        public string BusMovement { get => _busMovement; }

        public InfoAboutBus(string route,string busName,string busStop, string city,string movement)
        {
            _route = route;
            _busName = busName;
            _busStop = busStop;
            _busCity = city;
            _busMovement = movement;
        }
    }
}
