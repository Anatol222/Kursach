﻿namespace ProfileClassLibrary.BusClasses
{
    public class Day
    {
        private string _name;
        public string Name { get { return _name; } }

        public Day(string name)=>
            _name = name;
    }
}
