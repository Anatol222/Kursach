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

namespace Курсовая
{
    /// <summary>
    /// Логика взаимодействия для ConfirmEmail.xaml
    /// </summary>
    public partial class ConfirmEmail : Window
    {
        WorkWithInterface workWithInterface;
        ConfirmEmail confirmEmail;
        Registration registration;
        DataBase dataBase;
        public ConfirmEmail()
        {
            InitializeComponent();

            workWithInterface = new WorkWithInterface();
            confirmEmail = this; 
            registration = new Registration();
            dataBase = new DataBase();

            FirstSymbol.Focus();
            StaticRules();


        }
        
        private void SendAgain_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReturnRegistr_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
           string code = FirstSymbol.Text;
            code+=SecondSymbol.Text;
            code += ThirdSymbol.Text;
            code += FourthSymbol.Text;
            code += FifthSymbol.Text;

            if (code==Registration.CodeConfirm)
            {
                string querystring = $"insert into UserPersonalData(FirstName, LastName, Patronymic, Number) values('{Registration.G_FirsName}','{Registration.G_LastName}','{Registration.G_Patromic}','{Registration.G_Number}');";
                    //+ "INSERT INTO PersonalPassword(UserPersonalDataId,Password) VALUES((SELECT Id FROM UserPersonalData WHERE Number = '{Registration.G_Number}' AND FirstName = '{Registration.G_FirsName}' AND LastName = '{Registration.G_LastName}' AND Patronymic = '{Registration.G_Patromic}'),'{Registration.G_FirstPassword}');"
                    //+ "INSERT INTO PersonalLoginData(PersonalPasswordId,Email,ReserveEmail,UserPersonalDataId) VALUES((SELECT Id FROM PersonalPassword WHERE Password = '{Registration.G_FirstPassword}'),'{Registration.G_Email}','{EmailReserve.Text}',(SELECT Id FROM UserPersonalData WHERE Number = '{Registration.G_Number}' AND FirstName = '{Registration.G_FirsName}' AND LastName = '{Registration.G_LastName}' AND Patronymic = '{Registration.G_Patromic}')); ";
                SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
                dataBase.OpenConnection();
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Norm", "sex");
                }
                else
                    MessageBox.Show("(", "gg");
                querystring = $"INSERT INTO PersonalPassword(UserPersonalDataId,Password) VALUES((SELECT Id FROM UserPersonalData WHERE Number = '{Registration.G_Number}' AND FirstName = '{Registration.G_FirsName}' AND LastName = '{Registration.G_LastName}' AND Patronymic = '{Registration.G_Patromic}'),'{Registration.G_FirstPassword}');";
                command = new SqlCommand(querystring, dataBase.GetConnection());
                dataBase.OpenConnection();
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Norm", "sex");
                }
                else
                    MessageBox.Show("(", "gg");
                querystring = $"INSERT INTO PersonalLoginData(PersonalPasswordId,Email,ReserveEmail,UserPersonalDataId) VALUES((SELECT Id FROM PersonalPassword WHERE Password = '{Registration.G_FirstPassword}'),'{Registration.G_Email}','{EmailReserve.Text}',(SELECT Id FROM UserPersonalData WHERE Number = '{Registration.G_Number}' AND FirstName = '{Registration.G_FirsName}' AND LastName = '{Registration.G_LastName}' AND Patronymic = '{Registration.G_Patromic}')); ";
                command = new SqlCommand(querystring, dataBase.GetConnection());
                dataBase.OpenConnection();
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Norm", "sex");
                }
                else
                    MessageBox.Show("(", "gg");
                dataBase.CloseConnection();
            }
        }

        private void Cancellation_Click(object sender, RoutedEventArgs e)
        {
            
        }
        //workWithInterface.Cancellation(sender, e, confirmEmail);

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
            TextInfo.Content = workWithInterface.NotifyAboutCodeEmail;
            ReserveInfo.Content = workWithInterface.NotificationAboutReserveEmail;
        }
    }
}
