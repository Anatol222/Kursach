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
   
    public partial class Registration : Window
    {
        private WorkWithInterface workWithInterface;
        private Registration registration;
        private DataBase dataBase;
        private Email email;
        private NotificationWindow notificationWindow;

        private string _firstName, _lastName, _patronymic, _email, _number, _firstPassword, _secondPassword, _continuationEmail;

        private delegate void InvoceMessageBox(string messange);
        private event InvoceMessageBox Notification;
        
       

        public string FirstName_Text { get { return _firstName; } }
        public string LastName_Text { get { return _lastName; } }
        public string Patronymic_Text { get { return _patronymic; } }
        public string Email_Text { get { return _email; } }
        public string Number_Text { get { return _number; } }
        public string FirstPassword_Text { get { return _firstPassword; } }

        public Registration() => Constructor();
        public Registration(string firstName, string lastName, string patromic, string number, string password)
        {
            Constructor();

            _firstName = firstName;
            _lastName = lastName;
            _patronymic = patromic;
            _number = number;
            _firstPassword = password;
            _secondPassword = password;

             LastName.Text = _lastName;
             FirstName.Text = _firstName;
             Patronymic.Text = _patronymic;
             PhoneNumber.Text = _number;
             FirstPassword.Password = _firstPassword;
             ConfirmPassword.Password = _secondPassword;

             Email.Focus();

        }
        public void Constructor()
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

            Notification += Display;

            StaticMaxLenghtDataTable();
            FirstPassword.ToolTip = workWithInterface.NotificationPassword;
            foreach (var item in this.email.Emails)
            {
                ChoiceEmail.Items.Add(item);
            }
            ChoiceEmail.SelectedIndex = 5;
        }

        //Переделать
        private void Cancellation_Click(object sender, RoutedEventArgs e) =>
            workWithInterface.Cancellation(sender, e, registration);

        private void ComeIn_Click(object sender, RoutedEventArgs e) =>
            workWithInterface.SwitchAnotherWindon(sender, e, registration, new ComeIn());

        private void TextClear_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Clear();
            ((TextBox)sender).Foreground = Brushes.Black;
        }

        private void ViewPassword_TextChanged(object sender, TextChangedEventArgs e) =>
            FirstPassword.Password = ViewPassword.Text;

        //переделать
        private void ConfirmEmail_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                _lastName = LastName.Text;
                _firstName = FirstName.Text;
                _patronymic = Patronymic.Text;
                _email = Email.Text+_continuationEmail;
                _number = PhoneNumber.Text;
                _firstPassword = FirstPassword.Password;
                _secondPassword = ConfirmPassword.Password;
            }
            catch (Exception)
            {
                Notification?.Invoke( "Ошибка при регистрации");
            }

            if ((_lastName != "" && _lastName != "Ведите фамилию") && (_firstName != "" && _firstName != "Ведите имя")
                && (_patronymic != "" && _patronymic != "Ведите отчество") && (_email != "" && _email != "Ведите эл. почту")
                && (_number != "" && _number != "Ведите номер") && (_firstPassword != "") && (_secondPassword != "") && _number.Length>9)
            {
                if (workWithInterface.PasswordProcessing(_firstPassword, _secondPassword))
                {
                   
                    if (!Checkuser())
                    {
                        string codeConfirm = email.CreatingCodeConfirmation();
                        email.SendMessageConfirmationEmail(codeConfirm, Email_Text);
                        if (notificationWindow != null)
                            notificationWindow.Close();
                        workWithInterface.SwitchAnotherWindon(sender, e, registration, new ConfirmEmail(FirstName_Text,LastName_Text,Patronymic_Text,Number_Text,Email_Text,FirstPassword_Text,codeConfirm));
                    }
                }
                else
                    Notification?.Invoke("Разные пароли");
            }
            else
                Notification?.Invoke("Поля пустые");

        }
        private void PhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            workWithInterface.PhoneNumberProcessing(sender, e);

        private void Latters_PreviewTextInput(object sender, TextCompositionEventArgs e)=>
            workWithInterface.LattersProcessing(sender, e);

        //Переделать
        private void Email_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Trim();
            ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            if ((Convert.ToChar(e.Text) >= (char)48 && Convert.ToChar(e.Text) <= (char)57) || (Convert.ToChar(e.Text) >= (char)65 && Convert.ToChar(e.Text) <= (char)90) || (Convert.ToChar(e.Text) >= (char)97 && Convert.ToChar(e.Text) <= (char)122) || Convert.ToChar(e.Text)==46)
                e.Handled = false;
            else e.Handled = true;
        }
           
        //переделать
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)=> this.DragMove();

        private void ChoiceEmail_SelectionChanged(object sender, SelectionChangedEventArgs e)=>
            _continuationEmail = ChoiceEmail.SelectedItem.ToString();

        //переделать

        private bool Checkuser()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select * from PersonalLoginData where Email = '{Email_Text}'";
            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                Notification?.Invoke("Пользователь c такой почтой уже существует");
                return true;
            }
            else
            {
                querystring = $"SELECT * FROM UserPersonalData WHERE Number ='{Number_Text}'; ";
                command = new SqlCommand(querystring, dataBase.GetConnection());
                adapter.SelectCommand = command;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    Notification?.Invoke("Пользователь c таким номером уже существует");
                    return true;
                }
                return false;
            }
        }
        //переделать

        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            ViewPassword.Text = FirstPassword.Password;
            ShowOrHidePassword_Click(sender, e,Visibility.Visible, Visibility.Hidden, Visibility.Visible,Visibility.Hidden);
        }
        //переделать

        private void HidePassword_Click(object sender, RoutedEventArgs e)=>
            ShowOrHidePassword_Click(sender,e,Visibility.Hidden,Visibility.Visible,Visibility.Hidden,Visibility.Visible);

        //переделать
        private void Display(string messange)
        {
            if (notificationWindow != null)
                notificationWindow.Close();
            notificationWindow = new NotificationWindow(messange);
            notificationWindow.Show();
        }

        private void ShowOrHidePassword_Click(object sender, RoutedEventArgs e,Visibility view,Visibility firstPassword,Visibility hide,Visibility show)
        {
            ViewPassword.Visibility = view;
            FirstPassword.Visibility = firstPassword;
            HidePassword.Visibility = hide;
            ShowPassword.Visibility = show;
        }


        //переделать
        private void StaticMaxLenghtDataTable()
        {
            LastName.MaxLength = 20;
            FirstName.MaxLength = 20;
            Patronymic.MaxLength = 20;
            PhoneNumber.MaxLength = 16;
            FirstPassword.MaxLength = 16;
            Email.MaxLength = 35;
        }

        

       
    }
}
