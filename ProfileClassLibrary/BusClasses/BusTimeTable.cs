namespace ProfileClassLibrary.BusClasses
{
    public class BusTimeTable
    {
        public BusTimeTable(string nameBus, string route)
        {
            _nameBus = nameBus;
            _route = route;
        }
        private string _nameBus;
        public string NameBus { get => _nameBus; set => _nameBus = value; }

        private string _route;
        public string Route { get => _route; set => _route = value; }
    }
}
