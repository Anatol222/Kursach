using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileClassLibrary
{
    public class User : Person, IAccount
    {
        int _phone;
        string _email, _password, _reserveemail;
        public User(Gender gender, string name, string lastname, string patronymic,
                    DateTime birthDate,string password,string email,int phone)
                    : base(gender, name, lastname, patronymic, birthDate)
        {
            _password = password;
            _email = email;
            _phone = phone;
        }
        public string Password { get => _password;}
        public string Email { get => _email;}
        public string ReserveEmail { get => _reserveemail; }
        public int Phone { get => _phone;}
        public void ChangeLastName(string newLastName)
        {
            _lastname = newLastName;
        }
        public void ChangeName(string newName)
        {
            _name = newName;
        }
        public void ChangePassword(string currentPass,string newPass,string repeatNewPass)
        {
            if (Password == currentPass&& newPass == repeatNewPass)
            {
                _password = newPass;
            }
        }
        public void ChangePhone(int newPhone)
        {
            _phone = newPhone;
        }
        public void ChangePotronymic(string newPatronymic)
        {
            _patronymic = newPatronymic;
        }
        public void ChangeEmail(string email)
        {
            _email = email;
        }
        public void ChangeReserveEmail(string newResEmail)
        {
            _reserveemail = newResEmail;
        }
    }
}
