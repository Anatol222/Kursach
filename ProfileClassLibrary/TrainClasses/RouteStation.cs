namespace ProfileClassLibrary.TrainClasses
{
    public class RouteStation
    {
        public RouteStation(string station, string departure,string arrival,string trainStop)
        {
            _station = station;
            _departure = departure;
            _arrival = arrival;
            _trainStop = trainStop;
        }
        private string _station;
        public string Station { get { return _station; } }

        private string _departure;
        public string Departure { get { return _departure; } }

        private string _arrival;
        public string Arrival { get { return _arrival; } }

        private string _trainStop;
        public string TrainStop { get { return _trainStop; } }
    }
}
