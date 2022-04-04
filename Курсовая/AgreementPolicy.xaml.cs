using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AgreementPolicy.xaml
    /// </summary>
    public partial class AgreementPolicy : Window
    {
        private WorkWithInterface workWithInterface;
        public AgreementPolicy()
        {
            InitializeComponent();

            workWithInterface = new WorkWithInterface();

            AgrPolicy.Text = workWithInterface.Policy;
        }

        private void Cancellation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            ConfirmEmail.ConfirmRule = true;
            this.Close();
        }
    }
}
