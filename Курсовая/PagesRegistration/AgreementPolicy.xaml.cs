using System.Data.SqlClient;
using System.Windows;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;

namespace Курсовая.PagesRegistration
{
    
    public partial class AgreementPolicy : Window
    {
        private TextInterface workWithInterface;
        private DataBase dataBase;
        private AgreementPolicy agreementPolicy;

        private INavigation navigation;

        private delegate void InvoceMessageBox(string messange);
        private event InvoceMessageBox Notification;

        private string _firstName, _lastName, _patronymic, _number, _email, _password, _reserveEmail, _continuationEmail;

        public AgreementPolicy(string firstName, string lastName, string patronymic, string number, string email, string password,string reserveEmail, string continuationEmail)
        {
            InitializeComponent();

            workWithInterface = new TextInterface();
            dataBase = new DataBase();
            agreementPolicy = this;

            navigation = new ProgrammNavigation();

            _firstName = firstName;
            _lastName = lastName;
            _patronymic = patronymic;
            _number = number;
            _email = email;
            _password = password;
            _reserveEmail = reserveEmail;
            _continuationEmail = continuationEmail;

            Notification += navigation.Display;

            AgrPolicy.Text = workWithInterface.Policy;
        }

        private void Cancellation_Click(object sender, RoutedEventArgs e) =>
            navigation.Cancellation(agreementPolicy);

        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string querystring = $"insert into UserPersonalData(FirstName, LastName, Patronymic, Number) values('{_firstName}','{_lastName}','{_patronymic}','{_number}'); ";
            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
            dataBase.OpenConnection();
            try
            {
                if (await command.ExecuteNonQueryAsync() == 1)
                {
                    querystring = $"INSERT INTO PersonalPassword(UserPersonalDataId,Password) VALUES((SELECT Id FROM UserPersonalData WHERE Number = '{_number}' AND FirstName = '{_firstName}' AND LastName = '{_lastName}' AND Patronymic = '{_patronymic}'),'{_password}');";
                    command = new SqlCommand(querystring, dataBase.GetConnection());
                    if (command.ExecuteNonQuery() == 1)
                    {
                        if (_reserveEmail != "" && _reserveEmail != "Введите резервную эл. почту")
                            querystring = $"  INSERT INTO PersonalLoginData(PersonalPasswordId, Email, ReserveEmail, UserPersonalDataId) VALUES((SELECT PP.Id FROM PersonalPassword AS PP, UserPersonalData AS USD WHERE Password = '{_password}' AND PP.UserPersonalDataId = USD.Id AND USD.Number = '{_number}'),'{_email}','{_reserveEmail + _continuationEmail}',(SELECT Id FROM UserPersonalData WHERE Number = '{_number}' AND FirstName = '{_firstName}' AND LastName = '{_lastName}' AND Patronymic = '{_patronymic}')); ";
                        else
                            querystring = $"  INSERT INTO PersonalLoginData(PersonalPasswordId, Email, UserPersonalDataId) VALUES((SELECT PP.Id FROM PersonalPassword AS PP, UserPersonalData AS USD WHERE Password = '{_password}' AND PP.UserPersonalDataId = USD.Id AND USD.Number = '{_number}'),'{_email}',(SELECT Id FROM UserPersonalData WHERE Number = '{_number}' AND FirstName = '{_firstName}' AND LastName = '{_lastName}' AND Patronymic = '{_patronymic}')); ";
                        command = new SqlCommand(querystring, dataBase.GetConnection());
                        if (await command.ExecuteNonQueryAsync() == 1)
                        {
                            navigation.SwitchAnotherWindon(agreementPolicy, new MainFrame(_email));
                            StartWindow.startWindow.Close();
                        }
                        else
                            Notification?.Invoke("Не удается установить создать аккаунт");
                    }
                    else
                        Notification?.Invoke("Не удается установить пароль");
                }
                else
                    Notification?.Invoke("Не удается установить персональные даные");
            }
            catch {
                Notification?.Invoke("Неверный формат. Повторите попытку");
            }
            dataBase.CloseConnection();
        }
    }
}
