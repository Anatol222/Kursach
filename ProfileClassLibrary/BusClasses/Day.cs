namespace ProfileClassLibrary.BusClasses
{
    public class Day
    {
        public Day(string name)
        {
            _name = name;
        }
        private string _name;
        public string Name { get { return _name; } }
    }
}
