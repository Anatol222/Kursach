using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileClassLibrary.BusketClasses
{
    public class Bucket
    {
        public List<BucketItem> GetItems()
        {
            return new List<BucketItem>()
                {
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,true, TransportType.SityBus),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,false,TransportType.Train),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,true,TransportType.Bus),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,true,TransportType.SityBus),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,false,TransportType.Train),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,true,TransportType.Bus),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,false,TransportType.SityBus),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,true,TransportType.Train),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,true,TransportType.Plane),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,false,TransportType.Plane)
                };
        }
    }
}
