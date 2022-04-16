using System.Runtime.Serialization;

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
