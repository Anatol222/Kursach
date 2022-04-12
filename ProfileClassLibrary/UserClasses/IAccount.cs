using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileClassLibrary
{
    internal interface IAccount
    {
        string Email { get; }
        string ReserveEmail { get; }
        string Phone { get; }
        void ChangeName(string newName);
        void ChangeLastName(string newLastName);
        void ChangePotronymic(string newPatronymic);
        void ChangePhone(string newPhone);
        void ChangePassword(string pass,string newPass,string repeatNewPass);
        void ChangeEmail(string email);
        void ChangeReserveEmail(string newResEmail);
    }
}
