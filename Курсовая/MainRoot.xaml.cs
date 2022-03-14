using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Курсовая
{
    /// <summary>
    /// Логика взаимодействия для MainRoot.xaml
    /// </summary>
    public partial class MainRoot : Page
    {
        public static Window windowEntrance;
        public static InfoAboutProgramm infoWindow;

        public MainRoot()
        {
            InitializeComponent();
        }

        private void LinkVk_Click(object sender, RoutedEventArgs e) =>
            Process.Start("https://vk.com/anatol_prog");

        private void LinkInst_Click(object sender, RoutedEventArgs e) =>
            Process.Start("https://www.instagram.com/polesskie.brodyagi/");

        private void Registration_Click(object sender, RoutedEventArgs e)=>
            SetIn(sender, e, new Registration());

        private void Login_Click(object sender, RoutedEventArgs e)=>
            SetIn(sender, e, new ComeIn());

        private void SetIn(object sender, RoutedEventArgs e,Window window)
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
            if (infoWindow == null)
            {
                infoWindow = new InfoAboutProgramm();
                infoWindow.Show();
            }
            else infoWindow.Activate();
        }

        
    }
}
