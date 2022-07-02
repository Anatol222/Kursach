using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Tulpep.NotificationWindow;
using Курсовая.PagesComeIn;
using Курсовая.PagesRegistration;

namespace Курсовая
{
    public partial class MainRoot : Page
    {
        public static Window windowEntrance;
        public static InfoAboutProgramm infoWindow;
        private PopupNotifier popup = null;

        public MainRoot()=>
            InitializeComponent();

        private void LinkVk_Click(object sender, RoutedEventArgs e) =>
            Process.Start("https://vk.com/anatol_prog");

        private void LinkInst_Click(object sender, RoutedEventArgs e) =>
            Process.Start("https://www.instagram.com/polesskie.brodyagi/");

        private void Registration_Click(object sender, RoutedEventArgs e) =>
            SetIn(new Registration());

        private void Login_Click(object sender, RoutedEventArgs e) =>
            SetIn(new ComeIn());

        private void SetIn(Window window)
        {
            if (windowEntrance == null)
            {
                windowEntrance = window;
                windowEntrance.Show();
            }
            else if (windowEntrance != window)
            {
                windowEntrance.Close();
                windowEntrance = window; 
                windowEntrance.Show();
            }
            else
                windowEntrance.Activate();
        }

        private void CloseProgramm_Click(object sender, RoutedEventArgs e) =>
            Application.Current.Shutdown();

        private void CollapseProgramm_Click(object sender, RoutedEventArgs e) =>
            Application.Current.MainWindow.WindowState = WindowState.Minimized;

        private void InfoAboutProgramm_Click(object sender, RoutedEventArgs e)
        {
            if(infoWindow == null)
            {
                infoWindow = new InfoAboutProgramm();
                infoWindow.Show();
            }
            else infoWindow.Activate();
        }

        private void Minimise_Programm_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                Image image = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("../Images/maximize.png", UriKind.Relative);
                bitmap.EndInit();
                image.Source = bitmap;
                MinimiseBtn.Content = image;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                Image image = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("../Images/minimize.png", UriKind.Relative);
                bitmap.EndInit();
                image.Source = bitmap;
                MinimiseBtn.Content = image;
            }

        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popup.Popup();
            Clipboard.SetText("@Mr_Anatol");
        }

        private void MainRoot_Loaded(object sender, RoutedEventArgs e)
        {
            popup = new PopupNotifier();
            popup.Image = Properties.Resources.telegram;
            popup.ImageSize = new System.Drawing.Size(75, 75);
            popup.TitleText = "Уведомление!";
            popup.ContentText = "Ссылка на Telegram успешно скорирована!";
            popup.ShowGrip = false;
            popup.ShowOptionsButton = false;
        }
    }
}
