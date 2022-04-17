namespace ProfileClassLibrary
{
    public class User : Person, IAccount
    {
        private string _email;
        public User(string email) : base(email)
        {
            _email = email;
        }
        public string Password { get => _password;}
        public string Email { get => _email;}
        public string ReserveEmail { get => _reserveEmail; }
        public string Phone { get => _phoneNumber;}
        public void ChangeLastName(string newLastName)=>
            _lastName = newLastName;
        public void ChangeName(string newName)=>
            _firstName = newName;
        public void ChangePhone(string newPhone)=>
            _phoneNumber = newPhone;
        public void ChangePotronymic(string newPatronymic)=>
            _patronymic = newPatronymic;
        public void ChangeEmail(string email)=>
            _email = email;
        public void ChangeReserveEmail(string newResEmail)=>
            _reserveEmail = newResEmail;
        public void ChangePassword(string currentPass, string newPass, string repeatNewPass)
        {
            if (Password == currentPass && newPass == repeatNewPass)
                _password = newPass;
        }
    }
}
