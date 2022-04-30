namespace ProfileClassLibrary.BusClasses
{
    public class AllBusStop
    {
        public AllBusStop(string busStopName)
        {
            _busStopName = busStopName;
        }
        private string _busStopName;
        public string BusStopName { get => _busStopName; set => _busStopName = value; }
    }
}
