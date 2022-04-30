using System;

namespace ProfileClassLibrary.BusClasses
{
    public class Times
    {
        public Times(DateTime time)
        {
            _time = time;
        }
        private DateTime _time;
        public DateTime Time { get => _time; }
    }
}
