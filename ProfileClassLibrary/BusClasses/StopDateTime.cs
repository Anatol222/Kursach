using System;
using System.Collections.Generic;

namespace ProfileClassLibrary.BusClasses
{
    public class StopDateTime
    {
        public StopDateTime(string indexTime, List<DateTime> stopTimeList)
        {
            _stopTimeList = stopTimeList;
            _indexTime = indexTime;
        }
        private string _indexTime;
        public string IndexTime { get => _indexTime; }

        private List<DateTime> _stopTimeList;
        public List<DateTime> StopTimeList { get => _stopTimeList; }
    }
}
