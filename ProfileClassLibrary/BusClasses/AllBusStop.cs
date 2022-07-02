namespace ProfileClassLibrary.BusClasses
{
    public class AllBusStop
    {
        private string _busStopName;
        public string BusStopName { get => _busStopName; set => _busStopName = value; }

        public AllBusStop(string busStopName)=>
            _busStopName = busStopName;
    }
}
