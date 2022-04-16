using System.Runtime.Serialization;

namespace Курсовая
{ 
    [DataContract]
    internal class SaveUser
    {
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public bool IsComeIn { get; set; }
        public SaveUser(string email, string password, bool comeIn)
        {
            Email = email;
            Password = password;
            IsComeIn = comeIn;
        }
       
    }
}
