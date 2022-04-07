using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileClassLibrary
{
    public enum Gender
    {
        Male,
        Female
    }
    public abstract class Person
    {
        Gender _gender;
        protected string _name,_lastname,_patronymic;
        DateTime _birthDate;
        public Person(Gender gender, string name, string lastname, string patronymic, DateTime birthDate)
        {
            _gender = gender;
            _name = name;
            _lastname = lastname;
            _patronymic = patronymic;
            _birthDate = birthDate;
        }
        public int Id { get;set; }
        public string LastName
        {
            get { return _lastname; }
        }
        public string Patronymic
        {
            get { return _patronymic; }
        }
        public string Name
        {
            get { return _name;}
        }
        public Gender Gender
        {
            get { return _gender; }
        }
        public DateTime BirthDate
        {
            get { return _birthDate;}
        }
    }
}
