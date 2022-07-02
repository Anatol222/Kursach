using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ConsoleFree;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;

namespace Курсовая.PagesRegistration
{

    public partial class ConfirmEmail : Window
    {
        private TextInterface workWithInterface;
        private CipherPassword cipherPassword;
        private ConfirmEmail confirmEmail;
        private Email email;

        private IRegComeIn regComeIn;
        private INavigation navigation;
        private IDataProcessing dataProcessing;

        private string _firstName, _lastName, _patronymic, _number, _email, _password, _codeConfirm, _continuationEmail;

        private int _mm = 9, _ss = 60;
        private bool _ok = true;

        public ConfirmEmail(string firstName, string lastName, string patronymic, string number, string email, string password, string codeConfirm)
        {
            InitializeComponent();

            workWithInterface = new TextInterface();
            cipherPassword = new CipherPassword();
            confirmEmail = this;
            this.email = new Email();

            regComeIn = new RegComeIn();
            navigation = new ProgrammNavigation();
            dataProcessing = new DataProcessing();

            FirstSymbol.Focus();
            StaticRules();

            _firstName = firstName;
            _lastName = lastName;
            _patronymic = patronymic;
            _number = number;
            _email = email;
            _password = cipherPassword.encode(password);
            _codeConfirm = codeConfirm;

            StartTimer();
        }

        private void EmailReserve_GotFocus(object sender, RoutedEventArgs e)=>
            regComeIn.TextClear(sender, e);
        
        private void Email_PreviewTextInput(object sender, TextCompositionEventArgs e)=>
            dataProcessing.EmailTextInput(sender, e);

        private void Symbol_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            dataProcessing.SymbolProcessing(sender, e);
        
        private void Cancellation_Click(object sender, RoutedEventArgs e) =>
           navigation.Cancellation(confirmEmail);

        private void ReturnRegistr_Click(object sender, RoutedEventArgs e) =>
            navigation.SwitchAnotherWindon(confirmEmail, new Registration(_firstName, _lastName, _patronymic, _number, _password));

        private void SendAgain_Click(object sender, RoutedEventArgs e)
        {
            _codeConfirm = email.CreatingCodeConfirmation();
            email.SendMessageConfirmationEmail(_codeConfirm, _email);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string code = FirstSymbol.Text;
            code += SecondSymbol.Text;
            code += ThirdSymbol.Text;
            code += FourthSymbol.Text;
            code += FifthSymbol.Text;

            if (code == _codeConfirm)
                navigation.SwitchAnotherWindon(confirmEmail, new AgreementPolicy(_firstName, _lastName, _patronymic, _number, _email, _password, EmailReserve.Text, _continuationEmail));
            else
                MessageBox.Show("Неверный код");
        }

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
            if(_ok)
            TimerCode.Text = new DateTime(2003, 10, 20, 00, _mm, _ss).ToString(@"mm\:ss");
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

            EmailReserve.MaxLength = 35;

            foreach (var item in this.email.Emails)
                ChoiceEmail.Items.Add(item);
            ChoiceEmail.SelectedIndex = 5;
        }
    }
}
