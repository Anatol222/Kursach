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

        private string _firstName, _lastName, _patronymic, _email, _number, _firstPassword, _secondPassword;

        public Registration()
        {
            InitializeComponent();
            reg.Visibility = Visibility.Visible;
            Button_reg.Foreground = Brushes.Blue;

            log.Visibility = Visibility.Hidden;
            Button_log.Foreground = Brushes.Black;

            workWithInterface = new WorkWithInterface();
            registration = this;

            dataBase = new DataBase();

            MaxLenghtDataTable();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при регистрации", MessageBoxButton.OK);
            }

            //Обработать Пароль
            if ((_lastName != "" && _lastName != "Ведите фамилию") && (_firstName != "" &&_firstName!= "Ведите имя")
                && (_patronymic != "" && _patronymic!= "Ведите отчество") && (_email != "" && _email != "Ведите эл. почту") 
                && (_number != "" && _number != "Ведите номер")  && (_firstPassword != "")  && (_secondPassword != ""))
            {
                if (_firstPassword == _secondPassword)
                {
                    if (!checkuser())
                    {
                        string querystring = $"insert into UserPersonalData(FirstName, LastName, Patronymic, Number) values('{_firstName}','{_lastName}','{_patronymic}','{_number}')";
                        SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
                        dataBase.OpenConnection();
                        if (command.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Norm", "sex");
                        }
                        else
                            MessageBox.Show("(", "gg");
                        dataBase.CloseConnection();
                    }
                }
                else
                    MessageBox.Show("Разные пароли");
            }
            else
                MessageBox.Show("Поля пустые");
        }
        private Boolean checkuser()
        {
            var number = PhoneNumber.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select * from UserPersonalData where Number = '{number}'";

            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count>0)
            {
                MessageBox.Show("Пользователь существует");
                return true;
            }
            else return false;


        }

        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            ViewPassword.Text = FirstPassword.Password;
            ViewPassword.Visibility = Visibility.Visible;
            FirstPassword.Visibility = Visibility.Hidden;
            HidePassword.Visibility = Visibility.Visible;
            ShowPassword.Visibility = Visibility.Hidden;
        }

        private void HidePassword_Click(object sender, RoutedEventArgs e)
        {
            ViewPassword.Visibility = Visibility.Hidden;
            FirstPassword.Visibility = Visibility.Visible;
            HidePassword.Visibility = Visibility.Hidden;
            ShowPassword.Visibility = Visibility.Visible;
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
