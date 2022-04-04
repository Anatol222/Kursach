using ConsoleFree;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Runtime.Serialization.Json;

namespace Курсовая
{
    /// <summary>
    /// Логика взаимодействия для ComeIn.xaml
    /// </summary>
    public partial class ComeIn : Window
    {
        private WorkWithInterface workWithInterface;
        private ComeIn comeIn;
        private Email email;
        private DataBase dataBase;

        private string _email,_password;
        private bool  _saveUser,_reserveEmailExist;
        public ComeIn()
        {
            InitializeComponent();

            workWithInterface = new WorkWithInterface();
            comeIn = this;
            email = new Email();
            dataBase = new DataBase();

            InComeWithoutPassword();

            reg.Visibility = Visibility.Hidden;
            Button_reg.Foreground = Brushes.Black;

            log.Visibility = Visibility.Visible;
            Button_log.Foreground = Brushes.Blue;
            
            
        }

        private  void InComeWithoutPassword()
        {
            if (File.Exists("saveUser.json"))
            {
                DataContractJsonSerializer jsonF = new DataContractJsonSerializer(typeof(List<SaveUser>));
                List<SaveUser> dataUserSave = new List<SaveUser>();               
                using (FileStream fs = new FileStream("saveUser.json", FileMode.Open))
                    dataUserSave = (List<SaveUser>)jsonF.ReadObject(fs);
                if (dataUserSave[0].IsComeIn)
                {
                    Email.Text = dataUserSave[0].Email;
                    FirstPassword.Password = dataUserSave[0].Password;
                    CheckBoxSaveUser.IsChecked = true;
                    _saveUser = dataUserSave[0].IsComeIn;
                }
            }
            
        }

        private void Cancellation_Click(object sender, RoutedEventArgs e) =>
            workWithInterface.Cancellation(sender, e, comeIn);

        private void Registration_Click(object sender, RoutedEventArgs e)=>
            workWithInterface.SwitchAnotherWindon(sender, e, comeIn, new Registration());

        private void Email_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Trim();
            ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            if ((Convert.ToChar(e.Text) >= (char)48 && Convert.ToChar(e.Text) <= (char)57) || (Convert.ToChar(e.Text) >= (char)65 && Convert.ToChar(e.Text) <= (char)90) || (Convert.ToChar(e.Text) >= (char)97 && Convert.ToChar(e.Text) <= (char)122) || Convert.ToChar(e.Text) == (char)64 || Convert.ToChar(e.Text) == 46)
                e.Handled = false;
            else e.Handled = true;
        }
        private void TextClear_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Clear();
            ((TextBox)sender).Foreground = Brushes.Black;
        }

        private void UseReserveEmail_Click(object sender, RoutedEventArgs e)
        {
            if (ExaminationEmail())
            {
                string reserveEmail = default;
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();
                string querystring = $"SELECT ReserveEmail FROM PersonalLoginData WHERE Email ='{_email}'; ";
                SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
                adapter.SelectCommand = command;
                adapter.Fill(table);
                dataBase.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                        reserveEmail = reader.GetString(0);
                }
                catch (Exception)
                {
                    MessageBox.Show("Вы не указали резервную почту");
                    _reserveEmailExist = true;
                }
                dataBase.CloseConnection();
                if (!_reserveEmailExist)
                {
                    string reservePassword = email.CreatingPassword();
                    email.SendMessageNewPassword(reservePassword, reserveEmail);
                    workWithInterface.SwitchAnotherWindon(sender, e, comeIn, new ReserveInCome(reservePassword, reserveEmail, _email));
                }
            }
            else
                MessageBox.Show("Такого аккаунта не существует");
        }

        private void NewPassword_Click(object sender, RoutedEventArgs e)
        {
            if (ExaminationEmail())
            {
                string newPassword = email.CreatingPassword();
                string querystring = $"UPDATE PersonalPassword SET Password='{workWithInterface.encode(newPassword)}' WHERE PersonalPassword.Id= (SELECT PP.Id FROM PersonalPassword AS PP,PersonalLoginData AS PLD WHERE PLD.Email='{_email}' AND PLD.PersonalPasswordId=PP.Id); ";
                SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
                dataBase.OpenConnection();
                if (command.ExecuteNonQuery() == 1)
                {
                    email.SendMessageNewPassword(newPassword, _email);
                    MessageBox.Show("Новый пароль у вас на почте"); 
                }
                else
                    MessageBox.Show("Ошибка");
                dataBase.CloseConnection();
            }
            else
                MessageBox.Show("аккаунт с такой почтой не зарегистрирован");

        }


        private void InCome_Click(object sender, RoutedEventArgs e)
        {
            _email = Email.Text;
            _password = FirstPassword.Password;
            string extractPasswordFromBasaData = default;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $" SELECT Password FROM PersonalPassword WHERE Id = (SELECT PersonalPasswordId FROM PersonalLoginData WHERE Email ='{_email}'); ";

            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            dataBase.OpenConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
                extractPasswordFromBasaData = reader.GetString(0);

            if (table.Rows.Count == 1)
            {
                if (workWithInterface.decode(_password) == extractPasswordFromBasaData)
                {
                    if (_saveUser)
                    {
                        List<SaveUser> list = new List<SaveUser>()
                        {
                            new SaveUser(_email,_password,_saveUser)
                        };
                        File.WriteAllText("saveUser.json", JsonConvert.SerializeObject(list));
                    }
                    else
                        File.Delete("saveUser.json");
                    workWithInterface.SwitchAnotherWindon(sender, e, comeIn, new MainFrame()); 
                }
                else
                    MessageBox.Show("Пароль");
            }
            else
                MessageBox.Show("Такого аккаунта не существует");
            dataBase.CloseConnection();

        }

        private bool ExaminationEmail()
        {
            _email = Email.Text;
            _password = FirstPassword.Password;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select * from PersonalLoginData where Email = '{_email}'";
            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            return false;
        }

        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            ViewPassword.Text = FirstPassword.Password;
            ShowOrHidePassword_Click(sender, e, Visibility.Visible, Visibility.Hidden, Visibility.Visible, Visibility.Hidden);
        }
        //переделать

        private void HidePassword_Click(object sender, RoutedEventArgs e) =>
            ShowOrHidePassword_Click(sender, e, Visibility.Hidden, Visibility.Visible, Visibility.Hidden, Visibility.Visible);
        private void ShowOrHidePassword_Click(object sender, RoutedEventArgs e, Visibility view, Visibility firstPassword, Visibility hide, Visibility show)
        {
            ViewPassword.Visibility = view;
            FirstPassword.Visibility = firstPassword;
            HidePassword.Visibility = hide;
            ShowPassword.Visibility = show;
        }
        private void ViewPassword_TextChanged(object sender, TextChangedEventArgs e) =>
           FirstPassword.Password = ViewPassword.Text;
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void SaveUser_Click(object sender, RoutedEventArgs e)=>_saveUser= (bool)CheckBoxSaveUser.IsChecked;
    }
}
