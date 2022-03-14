using ConsoleFree;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Курсовая
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        private WorkWithInterface workWithInterface;
        private Registration registration;
        private DataBase dataBase;
        private Email email;

        private string _firstName, _lastName, _patronymic, _email, _number, _firstPassword, _secondPassword;

        public string FirstName_Text { get { return _firstName; } }
        public string LastName_Text { get { return _lastName; } }
        public string Patronymic_Text { get { return _patronymic; } }
        public string Email_Text { get { return _email; } }
        public string Number_Text { get { return _number; } }
        public string FirstPassword_Text { get { return _firstPassword; } }

        public static string G_FirsName {get;set;}
        public static string G_LastName {get;set;}
        public static string G_Patromic { get;set;}
        public static string G_Email { get;set;}
        public static  string G_Number { get;set;}
        public static string G_FirstPassword { get;set;}
        public static string CodeConfirm { get; set; }

        public Registration()
        {
            InitializeComponent();
            reg.Visibility = Visibility.Visible;
            Button_reg.Foreground = Brushes.Blue;

            log.Visibility = Visibility.Hidden;
            Button_log.Foreground = Brushes.Black;

            workWithInterface = new WorkWithInterface();
            registration = this;
            email = new Email();
            dataBase = new DataBase();

            MaxLenghtDataTable();

            FirstPassword.ToolTip = workWithInterface.NotificationPassword;
            
        }

        private void Cancellation_Click(object sender, RoutedEventArgs e) =>
            workWithInterface.Cancellation(sender, e, registration);

        private void ComeIn_Click(object sender, RoutedEventArgs e) =>
            workWithInterface.SwitchAnotherWindon(sender, e, registration, new ComeIn());

        private void TextClear_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Clear();
            ((TextBox)sender).Foreground = Brushes.Black;
        }


        private void ConfirmEmail_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                _lastName = LastName.Text;
                _firstName = FirstName.Text;
                _patronymic = Patronymic.Text;
                _email = Email.Text;
                _number = PhoneNumber.Text;
                _firstPassword = FirstPassword.Password;
                _secondPassword = ConfirmPassword.Password;

                G_FirsName = _firstName;
                G_LastName = _lastName;
                G_Email = _email;
                G_FirstPassword = _firstPassword;
                G_Patromic = _patronymic;
                G_Number = _number;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при регистрации", MessageBoxButton.OK);
            }

            if ((_lastName != "" && _lastName != "Ведите фамилию") && (_firstName != "" && _firstName != "Ведите имя")
                && (_patronymic != "" && _patronymic != "Ведите отчество") && (_email != "" && _email != "Ведите эл. почту")
                && (_number != "" && _number != "Ведите номер") && (_firstPassword != "") && (_secondPassword != ""))
            {
                if (workWithInterface.PasswordProcessing(_firstPassword, _secondPassword))
                {
                   
                    if (!checkuser())
                    {
                        CodeConfirm = email.CreatingCodeConfirmation();
                        email.SendMessageConfirmationEmail(CodeConfirm, Email_Text);
                        workWithInterface.SwitchAnotherWindon(sender, e, registration, new ConfirmEmail());

                        //string querystring = $"insert into UserPersonalData(FirstName, LastName, Patronymic, Number) values('{_firstName}','{_lastName}','{_patronymic}','{_number}')";
                        //SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
                        //dataBase.OpenConnection();
                        //if (command.ExecuteNonQuery() == 1)
                        //{
                        //    MessageBox.Show("Norm", "sex");
                        //}
                        //else
                        //    MessageBox.Show("(", "gg");
                        //dataBase.CloseConnection();
                    }
                }
                else
                    MessageBox.Show("Разные пароли");
            }
            else
                MessageBox.Show("Поля пустые");

        }
        private bool checkuser()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            //string querystring = $"SELECT * FROM UserPersonalData AS UP JOIN PersonalLoginData AS PL ON PL.UserPersonalDataId=UP.Id AND (PL.Email = '{_email}' OR UP.Number = '{PhoneNumber}')";
            //string querystring = $"select * from UserPersonalData where Number = '{number}'";
            string querystring = $"select * from PersonalLoginData where Email = '{_email}'";
            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count>0)
            {
                MessageBox.Show("Пользователь c такой почтой уже существует");
                return true;
            }
            else return false;
        }

        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            ViewPassword.Text = FirstPassword.Password;
            ShowOrHidePassword_Click(sender, e,Visibility.Visible, Visibility.Hidden, Visibility.Visible,Visibility.Hidden);
        }

        private void HidePassword_Click(object sender, RoutedEventArgs e)=>
            ShowOrHidePassword_Click(sender,e,Visibility.Hidden,Visibility.Visible,Visibility.Hidden,Visibility.Visible);
        private void ShowOrHidePassword_Click(object sender, RoutedEventArgs e,Visibility view,Visibility firstPassword,Visibility hide,Visibility show)
        {
            ViewPassword.Visibility = view;
            FirstPassword.Visibility = firstPassword;
            HidePassword.Visibility = hide;
            ShowPassword.Visibility = show;
        }
        //Добавить кодовое слово
        private void MaxLenghtDataTable()
        {
            LastName.MaxLength = 20;
            FirstName.MaxLength = 20;
            Patronymic.MaxLength = 20;
            PhoneNumber.MaxLength = 20;
            FirstPassword.MaxLength = 16;
            Email.MaxLength = 30;
        }

    }
}
