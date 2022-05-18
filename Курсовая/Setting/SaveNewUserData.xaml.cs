using System.Windows;
using System.Windows.Input;
using Курсовая.MainFrameForms;
using Курсовая.ProgrammInterface;

namespace Курсовая.Setting
{
    public partial class SaveNewUserData : Window
    {
        private INavigation navigation;
        private SaveNewUserData SNUD;
        public static bool CanRemoveFromBucket;
        public SaveNewUserData(string conten, string buttonText)
        {
            InitializeComponent();
            navigation = new ProgrammNavigation();
            SNUD = this;
            Confirm.Content = buttonText;
            NotificationBox.Text = conten;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (Confirm.Content.ToString() == "Да")
            {
                this.Close();
                new NotificationWindow("В ближайшее время будет оформлен возврат средств, ожидайте.").ShowDialog();
                CanRemoveFromBucket = true;
            }
            if (NotificationBox.Text == "Вы уверены, что хотите сохранить данные изменения?")
                ProfilePage.IsSaveNewData = true;
            if (NotificationBox.Text == "Не все поля заполнены. Нажмите продолжить, если не хотите ничего менять")
                ProfilePage.IsEmptyFields = false;
            if (NotificationBox.Text == "Вы уверены, что хотите приобрести билет?")
                SityBusPage.ConfirmBuyTicket = true;
            navigation.Cancellation(SNUD);

        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Cancellation_Click(object sender, RoutedEventArgs e)
        {
            CanRemoveFromBucket = false;
            navigation.Cancellation(SNUD);
            if (NotificationBox.Text == "Вы уверены, что хотите сохранить данные изменения?")
                ProfilePage.IsSaveNewData = false;
            if (NotificationBox.Text == "Не все поля заполнены. Нажмите продолжить, если не хотите ничего менять")
                ProfilePage.IsEmptyFields = true;
        }
    }
}
