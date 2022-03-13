using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Курсовая
{
    internal class WorkWithInterface:Window
    {
        public void Cancellation(object sender, RoutedEventArgs e)
        {
            Close();
            MainRoot.windowEntrance = null;
        }
    }
}
