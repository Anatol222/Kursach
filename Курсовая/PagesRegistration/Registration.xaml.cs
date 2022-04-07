using ConsoleFree;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Курсовая.PagesComeIn;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;

namespace Курсовая.PagesRegistration
{

    public partial class Registration : Window
    {
        private WorkWithInterface workWithInterface;
        private Registration registration;
        private DataBase dataBase;
        private Email email;

        private IRegComeIn regComeIn;
        private INavigation navigation;
        private IDataProcessing dataProcessing;

        private delegate void InvoceMessageBox(string messange);
        private event InvoceMessageBox Notification;

        private string _firstName, _lastName, _patronymic, _email, _number, _firstPassword, _secondPassword, _continuationEmail;

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
            Email.Focus();

            LastName.Text = _lastName = firstName;
            FirstName.Text = _firstName = lastName;
            Patronymic.Text = _patronymic = patromic;
            PhoneNumber.Text = _number = number;
            FirstPassword.Password = _firstPassword = password;
            ConfirmPassword.Password = _secondPassword = password;
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
            regComeIn = new RegComeIn();
            navigation = new ProgrammNavigation();
            dataProcessing = new DataProcessing();

            Notification += navigation.Display;

            StaticMaxLenghtDataTable();
            FirstPassword.ToolTip = workWithInterface.NotificationPassword;

            foreach (var item in this.email.Emails)
                ChoiceEmail.Items.Add(item);
            ChoiceEmail.SelectedIndex = 5;
        }

        private void ConfirmEmail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _lastName = LastName.Text;
                _firstName = FirstName.Text;
                _patronymic = Patronymic.Text;
                _email = Email.Text + _continuationEmail;
                _number = PhoneNumber.Text;
                _firstPassword = FirstPassword.Password;
                _secondPassword = ConfirmPassword.Password;
            }
            catch (Exception)
            {
                Notification?.Invoke("Ошибка при регистрации");
            }

            if ((_lastName != "" && _lastName != "Ведите фамилию") && (_firstName != "" && _firstName != "Ведите имя")
                && (_patronymic != "" && _patronymic != "Ведите отчество") && (_email != "" && _email != "Ведите эл. почту")
                && (_number != "" && _number != "Ведите номер") && (_firstPassword != "") && (_secondPassword != "") && _number.Length > 9)
            {
                if (dataProcessing.SatisfactionRulesPassword(_firstPassword, _secondPassword))
                {

                    if (!CheckuUser())
                    {
                        string codeConfirm = email.CreatingCodeConfirmation();
                        email.SendMessageConfirmationEmail(codeConfirm, Email_Text);
                        navigation.SwitchAnotherWindon(registration, new ConfirmEmail(FirstName_Text, LastName_Text, Patronymic_Text, Number_Text, Email_Text, FirstPassword_Text, codeConfirm));
                    }
                }
                else
                    Notification?.Invoke("Пароли не совпадают либо не удовлетворяет условию");
            }
            else
                Notification?.Invoke("Не все поля заполнены");

        }

        private void PhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            dataProcessing.PhoneNumberProcessing(sender, e);
        private void Latters_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            dataProcessing.LattersProcessing(sender, e);

        private void Cancellation_Click(object sender, RoutedEventArgs e) =>
            navigation.Cancellation(registration);
        private void ComeIn_Click(object sender, RoutedEventArgs e) =>
            navigation.SwitchAnotherWindon(registration, new ComeIn());

        private void Email_PreviewTextInput(object sender, TextCompositionEventArgs e)=>
            dataProcessing.EmailTextInput(sender, e);
        private void Password_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            dataProcessing.PasswordProcessing(sender, e);

        private void TextClear_GotFocus(object sender, RoutedEventArgs e) =>
           regComeIn.TextClear(sender, e);
        private void ShowOrHidePassword_Click(object sender, RoutedEventArgs e) =>
            regComeIn.ShowOrHidePassword(ViewPassword, FirstPassword, ShowOrHidePassword);
        private void ViewPassword_TextChanged(object sender, TextChangedEventArgs e) =>
            regComeIn.ViewChangingPassword(FirstPassword, ViewPassword);

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void ChoiceEmail_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            _continuationEmail = ChoiceEmail.SelectedItem.ToString();

        private bool CheckuUser()
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
