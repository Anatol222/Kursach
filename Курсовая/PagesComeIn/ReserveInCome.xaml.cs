using ConsoleFree;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;

namespace Курсовая.PagesComeIn
{
    public partial class ReserveInCome : Window
    {
        private CipherPassword cipherPassword;
        private  ReserveInCome inCome;
        private DataBase dataBase;
        private Email email;

        private INavigation navigation;
        private IDataProcessing dataProcessing;

        private delegate void InvoceMessageBox(string messange);
        private event InvoceMessageBox Notification;

        private string _reservePassword, _reserveEmail, _email;

        public ReserveInCome(string password, string reserveEmail, string email)
        {
            InitializeComponent();
            cipherPassword = new CipherPassword();
            inCome = this;
            dataBase = new DataBase();
            this.email = new Email();
            navigation = new ProgrammNavigation();
            dataProcessing = new DataProcessing();

            _reserveEmail = reserveEmail; 
            _email = email;
            _reservePassword = password;

            Notification = navigation.Display;

            FirstSymbol.Focus();
        }
        private void Symbol_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            dataProcessing.SymbolProcessing(sender,e);
        
        private void Cancellation_Click(object sender, RoutedEventArgs e) =>
            navigation.Cancellation(inCome);

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Retry_Click(object sender, RoutedEventArgs e)
        {
            _reservePassword = email.CreatingPassword();
            email.SendMessageNewPassword(_reservePassword, _reserveEmail);
            Notification?.Invoke("Мы отпраили повторный код");

        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string code = FirstSymbol.Text;
            code += SecondSymbol.Text;
            code += ThirdSymbol.Text;
            code += FourthSymbol.Text;
            code += FifthSymbol.Text;
            code += SixthSymbol.Text;
            code += SeventhSymbol.Text;
            code += EighthSymbol.Text;
             
            if (code==_reservePassword)
            {
                try
                {
                    string querystring = $"UPDATE PersonalPassword SET Password='{cipherPassword.encode(_reservePassword)}' WHERE PersonalPassword.Id= (SELECT PP.Id FROM PersonalPassword AS PP,PersonalLoginData AS PLD WHERE PLD.Email='{_email}' AND PLD.PersonalPasswordId=PP.Id); ";
                    SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
                    dataBase.OpenConnection();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        querystring = $"UPDATE PersonalLoginData SET Email = '{_reserveEmail}' WHERE Email = '{_email}'; ";
                        command = new SqlCommand(querystring, dataBase.GetConnection());
                        if (command.ExecuteNonQuery() == 1)
                            navigation.SwitchAnotherWindon(inCome, new MainFrame());
                        else
                            Notification?.Invoke("Не удается обновить резервную почту");
                    }
                    else
                        Notification?.Invoke("Не удается обновить пароль");
                }
                catch (System.Exception)
                {
                    Notification?.Invoke("Не удается обновить пароль");
                }
                dataBase.CloseConnection();
            }
            else
                Notification?.Invoke("Код не совпадает");
        }

        private void ChangeFocus_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SeventhSymbol.Text.Length == 1)
                EighthSymbol.Focus();
            else if (SixthSymbol.Text.Length == 1)
                SeventhSymbol.Focus();
            else if (FifthSymbol.Text.Length == 1)
                SixthSymbol.Focus();
            else if (FourthSymbol.Text.Length == 1)
                FifthSymbol.Focus();
            else if (ThirdSymbol.Text.Length == 1)
                FourthSymbol.Focus();
            else if (SecondSymbol.Text.Length == 1)
                ThirdSymbol.Focus();
            else if (FirstSymbol.Text.Length == 1)
                SecondSymbol.Focus();
        }


    }
}
