using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileClassLibrary.BusClasses
{
    public class Bus
    {
        public int Number { get; set; }
        public string Direction { get; set; }
        public Dictionary<string,List<BusStation>> Route { get; set; }

    }
}
