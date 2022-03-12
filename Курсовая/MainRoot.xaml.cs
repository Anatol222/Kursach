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
        public MainRoot()
        {
            
            InitializeComponent();
            
            
        }

        private void LinkVk_Click(object sender, RoutedEventArgs e) =>Process.Start("https://vk.com/anatol_prog");
    }
}
