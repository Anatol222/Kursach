using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Курсовая.MainFrameForms.PlanePages
{
    public partial class ClassChoiceWindow : Window
    {
        private string Choice;
        public ClassChoiceWindow()
        {
            InitializeComponent();
            IsReadyBtn.IsEnabled = false;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)=>
            this.Close();

        private void IsReadyBtn_Click(object sender, RoutedEventArgs e)
        {
            PlanePage.FlightClassString = Choice;
            this.Close();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Choice = ((TextBlock)((RadioButton)sender).Content).Text;
            IsReadyBtn.IsEnabled = true;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)=>
            this.DragMove();
    }
}
