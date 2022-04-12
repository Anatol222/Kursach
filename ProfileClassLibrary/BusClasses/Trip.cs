using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileClassLibrary.BusClasses
{
    public class Trip
    {
        public Dictionary<BusStation,DateTime> StoppingList { get; set; }

        public Trip()
        {

        }
    }
}
