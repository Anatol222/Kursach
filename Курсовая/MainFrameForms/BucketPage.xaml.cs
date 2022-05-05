using ProfileClassLibrary.BusketClasses;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Курсовая.Setting;
using System.Data.SqlClient;
using Курсовая.ProgrammInterface;
using MaterialDesignThemes.Wpf;
using System.Windows.Media;

namespace Курсовая.MainFrameForms
{
    public partial class BucketPage : Page
    {
        private DataBase dataBase;
        private IWorkWithBusList workWithBusList;
        private INavigation navigation;

        private delegate void InvoceMessageBox(string messange);
        private event InvoceMessageBox Notification;

        public List<BucketItem> _bucket;
        public static int BasketItemsCount { get; set; }
        public List<BucketItem> Bucket { get { return _bucket; } set { _bucket = value; } }
        public BucketPage()
        {
            InitializeComponent();

            DataContext = this;
            dataBase = new DataBase();
            workWithBusList = new WorkWithBusList();
            navigation = new ProgrammNavigation();

            Notification = navigation.Display;

            _bucket = (new Bucket()).GetItems(MainFrame.user.Email, dataBase.GetConnection());
            DeleteOverdueTickets();
            BasketItemsCount = _bucket.Count;

        }
        private async void DeleteOverdueTickets()
        {
            bool overueTicket = default;
            for (int i = 0; i < Bucket.Count; i++)
            {
                if (Bucket[i].TicketWhichTransport != 2)
                    if ((Bucket[i].DepartureDate <= DateTime.Now 
                        && ((Bucket[i].DepartureTime.Hour <= DateTime.Now.Hour && Bucket[i].DepartureTime.Minute <= DateTime.Now.Minute) || Bucket[i].DepartureTime.Hour < DateTime.Now.Hour)
                        && Bucket[i].DepartureDate.Day <= DateTime.Now.Day && Bucket[i].DepartureDate.Month <= DateTime.Now.Month && Bucket[i].DepartureDate.Year <= DateTime.Now.Year)
                        || (((Bucket[i].DepartureDate.Day < DateTime.Now.Day && Bucket[i].DepartureDate.Month <= DateTime.Now.Month)|| Bucket[i].DepartureDate.Month < DateTime.Now.Month) && Bucket[i].DepartureDate.Year <= DateTime.Now.Year))
                    {
                        string query = $"DELETE FROM ShoppingBasket WHERE IdPersonalLoginData = (SELECT Id FROM PersonalLoginData WHERE Email ='{MainFrame.user.Email}') " +
                            $"AND RouteTicket = '{Bucket[i].Direction}' AND TransportName ='{Bucket[i].TransportNumber}'" +
                            $" AND CountTickets ='{Bucket[i].TicketNum}' AND Id = {Bucket[i].Id};";
                        SqlCommand command = new SqlCommand(query, dataBase.GetConnection());
                        dataBase.OpenConnection();
                        try
                        {
                            await command.ExecuteNonQueryAsync();
                            Bucket.RemoveAt(i);
                            BucketListBox.Items.Refresh();
                            overueTicket = true;

                            MainFrame.BasketItemsCount -= 1;
                            MainFrame.basketButton.GetBindingExpression(TagProperty).UpdateTarget();
                            Badged badged = (Badged)MainFrame.basketButton.Template.FindName("basketItemsCount", MainFrame.basketButton);
                            badged.GetBindingExpression(Badged.BadgeProperty).UpdateTarget();
                        }
                        catch (Exception) { }
                        finally { dataBase.CloseConnection(); }
                        i--;
                    }
            }
            if (overueTicket)
                Notification?.Invoke("Из корзины был(и) удалены просроченные билет(ы). Если вы их не использавали опратитесь в поддержку");
        }


        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        private async void StatusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Bucket[BucketListBox.SelectedIndex]._purchaceStatus == true)
                new NotificationWindow("Билет уже куплен. Приятной поездки!").ShowDialog();
            else
            {
                Bucket[BucketListBox.SelectedIndex]._purchaceStatus = true;
                ((Image)(((Button)sender).Content)).GetBindingExpression(Image.SourceProperty).UpdateTarget();
                
                ListBoxItem myListBoxItem = (ListBoxItem)(BucketListBox.ItemContainerGenerator.ContainerFromItem(BucketListBox.Items.CurrentItem));
                // Getting the ContentPresenter of myListBoxItem
                ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListBoxItem);
                
                // Finding textBlock from the DataTemplate that is set on that ContentPresenter
                DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                Button RemoveBtn = (Button)myDataTemplate.FindName("RemoveBtn", myContentPresenter);
                ((Image)(RemoveBtn.Content)).GetBindingExpression(Image.SourceProperty).UpdateTarget();
                

                new NotificationWindow("Вы успешно оплатили билет. Приятной поездки!").ShowDialog();
                string query = $"UPDATE ShoppingBasket SET TicketStatus = 1 WHERE Id = {Bucket[BucketListBox.SelectedIndex].Id}; ";
                SqlCommand command = new SqlCommand(query, dataBase.GetConnection());
                dataBase.OpenConnection();
                try { await command.ExecuteNonQueryAsync(); }
                catch (Exception) { }
                finally { 
                    dataBase.CloseConnection();
                    _bucket = (new Bucket()).GetItems(MainFrame.user.Email, dataBase.GetConnection());
                    BucketListBox.ItemsSource = Bucket;
                }
            }
        }
        public void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {

            if (Bucket[BucketListBox.SelectedIndex]._purchaceStatus == true)
            {
                new SaveNewUserData("Вы уверены в том, что хотите отказаться от билета и вернуть средства?", "Да").ShowDialog();
                if (SaveNewUserData.CanRemoveFromBucket == true)
                {
                    DeleteFromBD();

                    MainFrame.BasketItemsCount -= 1;
                    MainFrame.basketButton.GetBindingExpression(TagProperty).UpdateTarget();
                    Badged badged = (Badged)MainFrame.basketButton.Template.FindName("basketItemsCount", MainFrame.basketButton);
                    badged.GetBindingExpression(Badged.BadgeProperty).UpdateTarget();
                }

            }
            else
            {
                DeleteFromBD();
                MainFrame.BasketItemsCount -= 1;
                MainFrame.basketButton.GetBindingExpression(TagProperty).UpdateTarget();
                Badged badged = (Badged)MainFrame.basketButton.Template.FindName("basketItemsCount", MainFrame.basketButton);
                badged.GetBindingExpression(Badged.BadgeProperty).UpdateTarget();
            }
        }
        private async void DeleteFromBD()
        {
            string query = $"DELETE FROM ShoppingBasket WHERE IdPersonalLoginData = (SELECT Id FROM PersonalLoginData WHERE Email ='{MainFrame.user.Email}') " +
                        $"AND RouteTicket = '{Bucket[BucketListBox.SelectedIndex].Direction}' AND TransportName ='{Bucket[BucketListBox.SelectedIndex].TransportNumber}'" +
                        $" AND CountTickets ='{Bucket[BucketListBox.SelectedIndex].TicketNum}' AND Id = {Bucket[BucketListBox.SelectedIndex].Id};";
            SqlCommand command = new SqlCommand(query, dataBase.GetConnection());
            dataBase.OpenConnection();
            try { await command.ExecuteNonQueryAsync(); }
            catch (Exception) { }
            finally { dataBase.CloseConnection(); }
            Bucket.RemoveAt(BucketListBox.SelectedIndex);
            BucketListBox.Items.Refresh();
        }

        private void BucketListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>
            workWithBusList.BucketListBoxFirst(sender, e);

    }
}
