using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileClassLibrary.PlaneClass
{
    [DataContract]
    public class Flight
    {
        [DataMember]
        public string flight { get; set; }
        [DataMember]
        public Airport airport { get; set; }
        [DataMember]
        public string plan { get; set; }
        [DataMember]
        public Status status { get; set; }
        [DataMember]
        public Airline airline { get; set; }
        
        [DataMember]
        public string[] numbers_gate { get; set; }
    }
    [DataContract]
    public class Airport
    {
        [DataMember]
        public string title { get; set; }
    }
    [DataContract]
    public class Status
    {
        [DataMember]
        public string title { get; set; }
    }
    [DataContract]
    public class Airline
    {
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public Logo logo { get; set; }
    }
    [DataContract]
    public class Logo
    {
        [DataMember]
        public string path { get; set; }
    } 
}
