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
    /// Логика взаимодействия для MainFrame.xaml
    /// </summary>
    public partial class MainFrame : Window
    {
        Uri ProfilePage = new Uri("MainFrameForms/ProfilePage.xaml", UriKind.RelativeOrAbsolute);
        Uri SityBusPage = new Uri("MainFrameForms/SityBusPage.xaml", UriKind.RelativeOrAbsolute);
        public MainFrame()
        {
            InitializeComponent();
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

            PagesNavigation.Navigate(ProfilePage);
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
            PagesNavigation.Navigate(ProfilePage);
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
