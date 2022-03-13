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
    /// <summary>
    /// Логика взаимодействия для ComeIn.xaml
    /// </summary>
    public partial class ComeIn : Window
    {
        private WorkWithInterface workWithInterface;
        private ComeIn comeIn;
        DataBase dataBase;

        public ComeIn()
        {
            InitializeComponent();

            reg.Visibility = Visibility.Hidden;
            Button_reg.Foreground = Brushes.Black;

            log.Visibility = Visibility.Visible;
            Button_log.Foreground = Brushes.Blue;
            
            workWithInterface = new WorkWithInterface();
            comeIn = this;

            dataBase = new DataBase();
        }

        private void Cancellation_Click(object sender, RoutedEventArgs e) =>
            workWithInterface.Cancellation(sender, e, comeIn);

        private void Registration_Click(object sender, RoutedEventArgs e)=>
            workWithInterface.SwitchAnotherWindon(sender, e, comeIn, new Registration());

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var email = Email.Text;
            //var pass = FirstPassword.Password;
            var email = 1;
            var pass = 1;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            //Зашлушка, поменять значения
            string querystring = $"insert into UserPersonalData(FirstName, LastName, Patronymic, Number) values('{email}','{pass}')";

            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count==1)
            {
                MessageBox.Show("успешно вошли","kek",MessageBoxButton.OK);

            }
            else
                MessageBox.Show("Такого аккаунта не существует", "kek", MessageBoxButton.OK);


        }
    }
}
