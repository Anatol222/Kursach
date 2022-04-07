using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace Курсовая
{
  
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
        }


        private void MaleBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FemaleBtn.Background ==Brushes.RosyBrown)
            {
                MaleBtn.Background = Brushes.Aqua;
                FemaleBtn.Background = Brushes.White;
            }
        }

        private void FemaleBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MaleBtn.Background == Brushes.Aqua)
            {
                FemaleBtn.Background = Brushes.RosyBrown;
                MaleBtn.Background = Brushes.White;
            }
        }
        private void DateTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789.".IndexOf(e.Text) < 0;
        }

        private void ToChangePassword_Click(object sender, RoutedEventArgs e)
        {
            Window changePassword = new ChangePassword();
            changePassword.Show();
        }
    }
}
