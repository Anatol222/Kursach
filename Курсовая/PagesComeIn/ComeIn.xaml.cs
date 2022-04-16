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
using Курсовая.PagesRegistration;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;

namespace Курсовая.PagesComeIn
{
    public partial class ComeIn : Window
    {
        private CipherPassword cipherPassword;
        private ComeIn comeIn;
        private Email email;
        private DataBase dataBase;

        private IRegComeIn regComeIn;
        private INavigation navigation;
        private IDataProcessing dataProcessing;

        private delegate void InvoceMessageBox(string messange);
        private event InvoceMessageBox Notification;

        private string _email,_password;
        private bool  _saveUser,_reserveEmailExist;

        public ComeIn()
        {
            InitializeComponent();

            cipherPassword = new CipherPassword();
            comeIn = this;
            email = new Email();
            dataBase = new DataBase();
            regComeIn = new RegComeIn();
            navigation = new ProgrammNavigation();
            dataProcessing = new DataProcessing();

            InComeWithoutPassword();

            Notification += navigation.Display;

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
            navigation.Cancellation(comeIn);
        private void Registration_Click(object sender, RoutedEventArgs e)=>
            navigation.SwitchAnotherWindon(comeIn, new Registration());

        private void Email_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            dataProcessing.EmailTextInputFull(sender, e);
        private void Password_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            dataProcessing.PasswordProcessing(sender, e);

        private void TextClear_GotFocus(object sender, RoutedEventArgs e) =>
            regComeIn.TextClear(sender,e);
        private void ShowOrHidePassword_Click(object sender, RoutedEventArgs e)=>
            regComeIn.ShowOrHidePassword(ViewPassword, FirstPassword, ShowOrHidePassword);
        private void ViewPassword_TextChanged(object sender, TextChangedEventArgs e) =>
           regComeIn.ViewChangingPassword(FirstPassword,ViewPassword);

        private void UseReserveEmail_Click(object sender, RoutedEventArgs e)
        {
            if (ExaminationEmail())
            {
                string reserveEmail = default;
                string querystring = $"SELECT ReserveEmail FROM PersonalLoginData WHERE Email ='{_email}'; ";
                SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
                dataBase.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    while (reader.Read())
                        reserveEmail = reader.GetString(0);
                    else
                        Notification?.Invoke("Вы не указали резервную почту");
                }
                catch (Exception)
                {
                    Notification?.Invoke("Вы не указали резервную почту");
                    _reserveEmailExist = true;
                }
                finally
                {
                    reader.Close();
                    dataBase.CloseConnection();
                    if (!_reserveEmailExist)
                    {
                        string reservePassword = email.CreatingPassword();
                        email.SendMessageNewPassword(reservePassword, reserveEmail);
                        navigation.SwitchAnotherWindon(comeIn, new ReserveInCome(reservePassword, reserveEmail, _email));
                    }
                }
            }
            else
                Notification?.Invoke("Такого аккаунта не существует");
        }

        private void NewPassword_Click(object sender, RoutedEventArgs e)
        {
            if (ExaminationEmail())
            {
                string newPassword = email.CreatingPassword();
                string querystring = $"UPDATE PersonalPassword SET Password='{cipherPassword.encode(newPassword)}' WHERE PersonalPassword.Id= (SELECT PP.Id FROM PersonalPassword AS PP,PersonalLoginData AS PLD WHERE PLD.Email='{_email}' AND PLD.PersonalPasswordId=PP.Id); ";
                SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
                dataBase.OpenConnection();
                try
                {
                    if (command.ExecuteNonQuery() == 1)
                    {
                        email.SendMessageNewPassword(newPassword, _email);
                        Notification?.Invoke("Новый пароль у вас на почте");
                    }
                    else
                        Notification?.Invoke("Не удается обновить пароль");
                }
                catch
                {
                    Notification?.Invoke("Ошибка при обновлении пароля");
                }
                dataBase.CloseConnection();
            }
            else
                Notification?.Invoke("Аккаунт с такой почтой не зарегистрирован");

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
                if (cipherPassword.decode(_password) == extractPasswordFromBasaData)
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
                    navigation.SwitchAnotherWindon(comeIn, new MainFrame(_email));
                    StartWindow.startWindow.Close();
                }
                else
                    Notification?.Invoke("Неверный пароль");
            }
            else
                Notification?.Invoke("Такого аккаунта не существует");
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

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void SaveUser_Click(object sender, RoutedEventArgs e)=>_saveUser= (bool)CheckBoxSaveUser.IsChecked;
    }
}
