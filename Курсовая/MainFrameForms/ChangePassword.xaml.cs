using System.Windows;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;
using System.Windows.Input;
using System.Windows.Controls;
using System.Data.SqlClient;

namespace Курсовая
{
    public partial class ChangePassword : Window
    {
        private CipherPassword cipherPassword;
        private DataBase dataBase;
        private ChangePassword changePassword;
        private TextInterface textInterface;

        private IDataProcessing dataProcessing;
        private IRegComeIn regComeIn;
        private INavigation navigation;

        private delegate void InvoceMessageBox(string message);
        private event InvoceMessageBox Notification;

        public static bool IsChangePasswor { get; private set; }

        public ChangePassword()
        {
            InitializeComponent();

            dataProcessing = new DataProcessing();
            regComeIn = new RegComeIn();
            navigation = new ProgrammNavigation();

            cipherPassword = new CipherPassword();
            dataBase = new DataBase();
            changePassword = this;
            textInterface = new TextInterface();

            NewPasswordBox.ToolTip = textInterface.NotificationPassword;

            Notification += navigation.Display;
        }

        private void PasswordChacking_PreviewTextInput(object sender, TextCompositionEventArgs e)=>
            dataProcessing.PasswordProcessing(sender, e);

        private void NewTextBox_TextChanged(object sender, TextChangedEventArgs e)=>
            regComeIn.ViewChangingPassword(NewPasswordBox, NewTextBox);

        private void OldTextBox_TextChanged(object sender, TextChangedEventArgs e)=>
            regComeIn.ViewChangingPassword(OldPasswordBox, OldTextBox);

        private void SavePassword_Click(object sender, RoutedEventArgs e)
        {
            if (Password())
            {
                IsChangePasswor = true;
                navigation.Cancellation(changePassword); 
            }
        }

        private bool Password()
        {
            if (OldPasswordBox.Password == cipherPassword.decode(MainFrame.user.Password))
            {

                if (dataProcessing.SatisfactionRulesPassword(NewPasswordBox.Password,ConfirmNewPasswordBox.Password))
                {
                    try
                    {
                        string query = $"UPDATE PersonalPassword SET Password='{cipherPassword.encode(NewPasswordBox.Password)}' WHERE PersonalPassword.Id= (SELECT PP.Id FROM PersonalPassword AS PP,PersonalLoginData AS PLD WHERE PLD.Email='{MainFrame.user.Email}' AND PLD.PersonalPasswordId=PP.Id); ";
                        SqlCommand sqlCommand = new SqlCommand(query, dataBase.GetConnection());
                        dataBase.OpenConnection();
                        if (sqlCommand.ExecuteNonQuery() == 1)
                            return true;
                        else
                            Notification?.Invoke("Не удается обновить пароль");
                    }
                    catch (System.Exception) { return false;}
                }
                else
                    Notification?.Invoke("Новый пароль не совпадает, или не соответствует требованием");
            }
            else
                Notification?.Invoke("Старый пароль не совпадает");
            return false;
        }

        private void ShowOrHidePassword_Click(object sender, RoutedEventArgs e) =>
            regComeIn.ShowOrHidePassword(OldTextBox, OldPasswordBox, ShowOrHidePassword);

        private void ShowOrHidТNewePassword_Click(object sender, RoutedEventArgs e) =>
            regComeIn.ShowOrHidePassword(NewTextBox, NewPasswordBox, ShowOrHideNewPassword);
    }
}
