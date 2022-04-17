using ProfileClassLibrary.BusketClasses;
using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Курсовая.Setting;

namespace Курсовая.MainFrameForms
{
    /// <summary>
    /// Логика взаимодействия для BusketPage.xaml
    /// </summary>
    public partial class BucketPage : Page 
    {
        public BucketPage()
        {
            InitializeComponent();
            DataContext = this;
        }
        public List<BucketItem> _bucket= (new Bucket()).GetItems();


        public List<BucketItem> Bucket { get { return _bucket; } set { _bucket = value;} }

        private void StatusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Bucket[BucketListBox.SelectedIndex]._purchaceStatus == true)
            {
                new NotificationWindow("Билет уже куплен. Приятной поездки!").ShowDialog();
            }
            else
            {
                new NotificationWindow("Вы успешно оплатили билет. Приятной поездки!").ShowDialog();
            }
        }
        public void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Bucket[BucketListBox.SelectedIndex]._purchaceStatus == true)
            {
                new SaveNewUserData("Вы уверены в том, что хотите отказаться от билета и вернуть средства?", "Да").ShowDialog();
                if (SaveNewUserData.CanRemoveFromBucket == true)
                {
                    Bucket.RemoveAt(BucketListBox.SelectedIndex);
                    BucketListBox.Items.Refresh();
                }
            }
            else
            {

                Bucket.RemoveAt(BucketListBox.SelectedIndex);
                BucketListBox.Items.Refresh();
            }
            
            
        }

        private void BucketListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (null == listBox)
            {
                return;
            }

            var point = e.GetPosition((UIElement)sender);

            VisualTreeHelper.HitTest(listBox, null, (hitTestResult) =>
            {
                var uiElement = hitTestResult.VisualHit as UIElement;

                while (null != uiElement)
                {
                    var listBoxItem = uiElement as ListBoxItem;
                    if (null != listBoxItem)
                    {
                        listBoxItem.IsSelected = true;
                        return HitTestResultBehavior.Stop;
                    }

                    uiElement = VisualTreeHelper.GetParent(uiElement) as UIElement;
                }

                return HitTestResultBehavior.Continue;
            }, new PointHitTestParameters(point));
        }
    }
}
