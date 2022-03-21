using System;
using System.Collections.Generic;
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
using ConsoleFree;


namespace Курсовая
{

    public partial class ConfirmEmail : Window
    {
        private WorkWithInterface workWithInterface;
        private ConfirmEmail confirmEmail;
        private DataBase dataBase;
        private Email email;
        private NotificationWindow notificationWindow;

        private string _firstName, _lastName, _patronymic, _number, _email, _password, _codeConfirm, _continuationEmail;

        public ConfirmEmail(string firstName, string lastName, string patronymic, string number, string email, string password, string codeConfirm)
        {
            InitializeComponent();

            workWithInterface = new WorkWithInterface();
            confirmEmail = this;
            dataBase = new DataBase();
            this.email = new Email();

            FirstSymbol.Focus();
            StaticRules();

            Notification = Display;

            EmailReserve.MaxLength = 35;

            _firstName = firstName;
            _lastName = lastName;
            _patronymic = patronymic;
            _number = number;
            _email = email;
            _password = password;
            _codeConfirm = codeConfirm;

            foreach (var item in this.email.Emails)
                ChoiceEmail.Items.Add(item);
            ChoiceEmail.SelectedIndex = 5;
        }

        private delegate void InvoceMessageBox(string messange);
        private event InvoceMessageBox Notification;
       
        private void EmailReserve_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Clear();
            ((TextBox)sender).Foreground = Brushes.Black;
        }

        private void SendAgain_Click(object sender, RoutedEventArgs e)
        {
            _codeConfirm = email.CreatingCodeConfirmation();
            email.SendMessageConfirmationEmail(_codeConfirm, _email);
        }

        private void Symbol_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if ((Convert.ToChar(e.Text) >= (char)97 && Convert.ToChar(e.Text) <= (char)122) || (Convert.ToChar(e.Text) >= (char)65 && Convert.ToChar(e.Text) <= (char)90) ||(Convert.ToChar(e.Text) >= (char)48 && Convert.ToChar(e.Text) <= (char)57))
            {
                ((TextBox)sender).Text = e.Text.ToUpper();
                ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            }
                e.Handled = true;

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void ReturnRegistr_Click(object sender, RoutedEventArgs e)=>
            workWithInterface.SwitchAnotherWindon(sender, e, confirmEmail, new Registration(_firstName,_lastName,_patronymic,_number,_password));

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string code = FirstSymbol.Text;
            code += SecondSymbol.Text;
            code += ThirdSymbol.Text;
            code += FourthSymbol.Text;
            code += FifthSymbol.Text;

            if (code == _codeConfirm)
            {
                string querystring = $"insert into UserPersonalData(FirstName, LastName, Patronymic, Number) values('{_firstName}','{_lastName}','{_patronymic}','{_number}'); ";
                SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
                dataBase.OpenConnection();
                if (command.ExecuteNonQuery() == 1)
                {
                    querystring = $"INSERT INTO PersonalPassword(UserPersonalDataId,Password) VALUES((SELECT Id FROM UserPersonalData WHERE Number = '{_number}' AND FirstName = '{_firstName}' AND LastName = '{_lastName}' AND Patronymic = '{_patronymic}'),'{_password}');";
                    command = new SqlCommand(querystring, dataBase.GetConnection());
                    if (command.ExecuteNonQuery() == 1)
                    {
                        if  (EmailReserve.Text !="")
                            querystring = $"  INSERT INTO PersonalLoginData(PersonalPasswordId, Email, ReserveEmail, UserPersonalDataId) VALUES((SELECT PP.Id FROM PersonalPassword AS PP, UserPersonalData AS USD WHERE Password = '{_password}' AND PP.UserPersonalDataId = USD.Id AND USD.Number = '{_number}'),'{_email}','{EmailReserve.Text+ _continuationEmail}',(SELECT Id FROM UserPersonalData WHERE Number = '{_number}' AND FirstName = '{_firstName}' AND LastName = '{_lastName}' AND Patronymic = '{_patronymic}')); ";
                        else
                            querystring = $"  INSERT INTO PersonalLoginData(PersonalPasswordId, Email, UserPersonalDataId) VALUES((SELECT PP.Id FROM PersonalPassword AS PP, UserPersonalData AS USD WHERE Password = '{_password}' AND PP.UserPersonalDataId = USD.Id AND USD.Number = '{_number}'),'{_email}',(SELECT Id FROM UserPersonalData WHERE Number = '{_number}' AND FirstName = '{_firstName}' AND LastName = '{_lastName}' AND Patronymic = '{_patronymic}')); ";
                        command = new SqlCommand(querystring, dataBase.GetConnection());
                        if (command.ExecuteNonQuery() == 1)
                            MessageBox.Show("Norm");
                        else
                            Notification?.Invoke("gg");
                        if (notificationWindow != null)
                            notificationWindow.Close();
                    }
                else
                        Notification?.Invoke("gg");
                }
                else
                    Notification?.Invoke( "gg");
                dataBase.CloseConnection();
            }
            code = default;
        }
        private void Display(string messange)
        {
            if (notificationWindow != null)
                notificationWindow.Close();
            notificationWindow = new NotificationWindow(messange);
            notificationWindow.Show();
        }
        private void Cancellation_Click(object sender, RoutedEventArgs e)=>
           workWithInterface.Cancellation(sender, e, confirmEmail);
        private void ChoiceEmail_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            _continuationEmail = ChoiceEmail.SelectedItem.ToString();
        private void ChangeFocus_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FourthSymbol.Text.Length == 1)
                FifthSymbol.Focus();
            else if (ThirdSymbol.Text.Length==1)
                FourthSymbol.Focus();
            else if (SecondSymbol.Text.Length==1)
                ThirdSymbol.Focus();
            else if (FirstSymbol.Text.Length==1)
                SecondSymbol.Focus();
        }
        private void StaticRules()
        {
            FirstSymbol.MaxLength = 1;
            SecondSymbol.MaxLength = 1;
            ThirdSymbol.MaxLength = 1;
            FourthSymbol.MaxLength = 1;
            FifthSymbol.MaxLength = 1;
            TextInfo.Text = workWithInterface.NotifyAboutCodeEmail;
            ReserveInfo.Text = workWithInterface.NotificationAboutReserveEmail;
        }
        private void Email_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Trim();
            ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            if ((Convert.ToChar(e.Text) >= (char)48 && Convert.ToChar(e.Text) <= (char)57) || (Convert.ToChar(e.Text) >= (char)65 && Convert.ToChar(e.Text) <= (char)90) || (Convert.ToChar(e.Text) >= (char)97 && Convert.ToChar(e.Text) <= (char)122) || Convert.ToChar(e.Text) == 46)
                e.Handled = false;
            else e.Handled = true;
        }
    }
}
