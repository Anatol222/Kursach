using ConsoleFree;
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
    /// Логика взаимодействия для ReserveInCome.xaml
    /// </summary>
    public partial class ReserveInCome : Window
    {
        private WorkWithInterface workWithInterface;
        private  ReserveInCome inCome;
        private DataBase dataBase;
        private Email email;
        private string _reservePassword, _reserveEmail, _email;
        public ReserveInCome(string password, string reserveEmail, string email)
        {
            InitializeComponent();
            workWithInterface = new WorkWithInterface();
            inCome = this;
            dataBase = new DataBase();
            this.email = new Email();

            _reserveEmail = reserveEmail; 
            _email = email;
            _reservePassword = password;

            FirstSymbol.Focus();
        }
        private void Symbol_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if ((Convert.ToChar(e.Text) >= (char)97 && Convert.ToChar(e.Text) <= (char)122) || (Convert.ToChar(e.Text) >= (char)65 && Convert.ToChar(e.Text) <= (char)90) || (Convert.ToChar(e.Text) >= (char)48 && Convert.ToChar(e.Text) <= (char)57))
            {
                ((TextBox)sender).Text = e.Text.ToUpper();
                ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            }
            e.Handled = true;

        }
        private void ChangeFocus_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SeventhSymbol.Text.Length ==1)
                EighthSymbol.Focus();
            else if (SixthSymbol.Text.Length ==1)
                SeventhSymbol.Focus();
            else if (FifthSymbol.Text.Length==1)
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

        private void Retry_Click(object sender, RoutedEventArgs e)
        {
            _reservePassword = email.CreatingPassword();
            email.SendMessageNewPassword(_reservePassword, _reserveEmail);
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
                string querystring = $"UPDATE PersonalPassword SET Password='{workWithInterface.encode(_reservePassword)}' WHERE PersonalPassword.Id= (SELECT PP.Id FROM PersonalPassword AS PP,PersonalLoginData AS PLD WHERE PLD.Email='{_email}' AND PLD.PersonalPasswordId=PP.Id); ";
                SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
                dataBase.OpenConnection();
                if (command.ExecuteNonQuery() == 1)
                {
                    querystring = $"UPDATE PersonalLoginData SET Email = '{_reserveEmail}' WHERE Email = '{_email}'; ";
                    command = new SqlCommand(querystring, dataBase.GetConnection());
                    if (command.ExecuteNonQuery() == 1)
                        workWithInterface.SwitchAnotherWindon(sender, e, inCome, new MainFrame());
                    else
                        MessageBox.Show("gg");
                }
                else
                    MessageBox.Show("gg");
                dataBase.CloseConnection();
            }
            else
                MessageBox.Show("не тот код");
        }
        private void Cancellation_Click(object sender, RoutedEventArgs e) =>
           workWithInterface.Cancellation(sender, e, inCome);
    }
}
