using System;

namespace ProfileClassLibrary.BusClasses
{
    public class Times
    {
        private DateTime _time;
        public DateTime Time { get => _time; }

        public Times(DateTime time)=>
            _time = time;
    }
}
