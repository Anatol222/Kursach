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
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,true),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,false),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,true),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,true),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,false),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,true),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,false),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,true),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,true),
                    new BucketItem("rqwrrewwe-erwrewrwerwe",DateTime.Now,DateTime.Now,"B232",1,false)
                };
        }
    }
}
