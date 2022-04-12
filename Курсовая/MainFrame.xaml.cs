using ProfileClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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
using Курсовая.Setting;

namespace Курсовая
{
    /// <summary>
    /// Логика взаимодействия для MainFrame.xaml
    /// </summary>
    public partial class MainFrame : Window
    {
        Uri TrainPage = new Uri("MainFrameForms/TrainPage.xaml", UriKind.RelativeOrAbsolute);
        public static MainFrame mainFrame;
        public static User user;
        private string _email { get; }
        private bool ALTtrue;
        public MainFrame()
        {
            InitializeComponent();
            ChangeData();
        }
        public MainFrame(string email)
        {
            InitializeComponent();
            _email = email;
            user = new User(_email);
            ChangeData();
            
            
        }
        public void ChangeData()
        {
            mainFrame = this;
            FNamePatronymic.Content = user.Name + " " + user.Patronymic;
            EmailUres.Content = user.Email;
            if (File.Exists("UserIcon.json"))
            {
                DataContractJsonSerializer jsonF = new DataContractJsonSerializer(typeof(List<IconUser>));
                List<IconUser> dataUserSave = new List<IconUser>();
                using (FileStream fs = new FileStream("UserIcon.json", FileMode.Open))
                    dataUserSave = (List<IconUser>)jsonF.ReadObject(fs);
                Image image = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(dataUserSave[0].IconAn, UriKind.Relative);
                bitmap.EndInit();
                image.Source = bitmap;
                OpenSetting.Content = image;
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void rdHome_Click(object sender, RoutedEventArgs e)
        {
            // PagesNavigation.Navigate(new HomePage());

            PagesNavigation.Navigate(TrainPage);
        }
        private void rdSounds_Click(object sender, RoutedEventArgs e)
        {
        }

        private void PagesNavigation_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(TrainPage);
        }

        private void OpenSetting_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new ProfilePage());
        }

        private void UpateInfoUser_Click(object sender, RoutedEventArgs e)
        {
            ChangeData();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (WindowState == WindowState.Normal)
                    WindowState = WindowState.Maximized;
                else
                    WindowState = WindowState.Normal;
            }
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl|| ALTtrue == true)
            {
                if (e.Key == Key.Q)
                    Close();
                else if (e.Key == Key.M)
                    WindowState = WindowState.Minimized;
                else if (e.Key == Key.D6)
                    PagesNavigation.Navigate(new ProfilePage());
                //else if (e.Key == Key.D1)
                //    PagesNavigation.Navigate();
                //else if (e.Key == Key.D2)
                //    PagesNavigation.Navigate();
                //else if (e.Key == Key.D3)
                //    PagesNavigation.Navigate());
                //else if (e.Key == Key.D4)
                //    PagesNavigation.Navigate();
                //else if (e.Key == Key.D5)
                //    PagesNavigation.Navigate();
                ALTtrue = !ALTtrue;
            }
        }

        private void rdNotes_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(SityBusPage);
        }


        //private void rdSounds_Click(object sender, RoutedEventArgs e)
        //{
        //    PagesNavigation.Navigate(new System.Uri("Pages/SoundsPage.xaml", UriKind.RelativeOrAbsolute));
        //}

        //private void rdNotes_Click(object sender, RoutedEventArgs e)
        //{
        //    PagesNavigation.Navigate(new System.Uri("Pages/NotesPage.xaml", UriKind.RelativeOrAbsolute));
        //}

        //private void rdPayment_Click(object sender, RoutedEventArgs e)
        //{
        //    PagesNavigation.Navigate(new System.Uri("Pages/PaymentPage.xaml", UriKind.RelativeOrAbsolute));
        //}
    }
}
