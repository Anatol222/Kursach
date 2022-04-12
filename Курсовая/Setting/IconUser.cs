using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая.Setting
{
    [DataContract]
    internal class IconUser
    {
        [DataMember]
        public string IconAn { get; set; }
        public IconUser(string icon)
        {
            IconAn = icon;
        }
    }
}
