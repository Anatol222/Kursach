using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Курсовая.ProgrammInterface;
using ConsoleFree;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Threading;

namespace Курсовая.Setting
{
    
    public partial class ConfirmEmailWinow : Window
    {
        private Email email;
        private DataBase dataBase;
        private TextInterface textInterface;

        private INavigation navigation;
        private IDataProcessing dataProcessing;

        private delegate void InvoceMessageBox(string message);
        private event InvoceMessageBox Notification;

        private string _codeConfirm,_newEmail;
        private int _mm = 9, _ss = 60;
        private bool _ok = true;
        public static bool IsReplecement { get; set; }

        public ConfirmEmailWinow(string newEmail)
        {
            InitializeComponent();
            navigation = new ProgrammNavigation();
            dataProcessing = new DataProcessing();
            email = new Email();
            dataBase = new DataBase();
            textInterface = new TextInterface();

            TextInfo.Text = textInterface.NotifyAboutCodeEmail;
            Notification += navigation.Display;
            _newEmail = newEmail;
            SendMessageEmail();
            StartTimer();
        }

        private void Cancellation_Click(object sender, RoutedEventArgs e) =>
           navigation.Cancellation(this);

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string code = FirstSymbol.Text;
            code += SecondSymbol.Text;
            code += ThirdSymbol.Text;
            code += FourthSymbol.Text;
            code += FifthSymbol.Text;

            if (code == _codeConfirm)
            {
                try
                {
                    string query = $"UPDATE PersonalLoginData SET Email = '{_newEmail}' WHERE Email = '{MainFrame.user.Email}'; ";
                    SqlCommand sqlCommand = new SqlCommand(query, dataBase.GetConnection());
                    dataBase.OpenConnection();
                    if (sqlCommand.ExecuteNonQuery() != 1)
                        Notification?.Invoke("Не удается обновить почту");
                    else
                    {
                        dataBase.CloseConnection();
                        File.Delete("saveUser.json");
                        IsReplecement = true;
                        navigation.Cancellation(this);
                    }
                }
                catch (Exception){ Notification?.Invoke("Не удается обновить почту");}
                dataBase.CloseConnection();
            }
            else
                Notification?.Invoke("Неверный код");
        }
        private void SendAgain_Click(object sender, RoutedEventArgs e) => 
            SendMessageEmail();
            
        private void ChangeFocus_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FourthSymbol.Text.Length == 1)
                FifthSymbol.Focus();
            else if (ThirdSymbol.Text.Length == 1)
                FourthSymbol.Focus();
            else if (SecondSymbol.Text.Length == 1)
                ThirdSymbol.Focus();
            else if (FirstSymbol.Text.Length == 1)
                SecondSymbol.Focus();
        }
        private void Symbol_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            dataProcessing.SymbolProcessing(sender, e);

        private void SendMessageEmail()
        {
            _codeConfirm = email.CreatingCodeConfirmation();
            email.SendMessageConfirmationEmail(_codeConfirm, _newEmail);
        }
        private void StartTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _ss--;
            if (_ss == 00)
            {
                _ss = 59;
                _mm--;
                if (_mm == -1)
                {
                    _ok = false;
                    TimerCode.Text = "Время вышло";
                    _codeConfirm = default;
                    ((DispatcherTimer)sender).Stop();
                }
            }
            if (_ok)
                TimerCode.Text = new DateTime(2003, 10, 20, 00, _mm, _ss).ToString(@"mm\:ss");
        }
    }
}
