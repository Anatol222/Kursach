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
        public void Cancellation(object sender, RoutedEventArgs e,Window window)
        {
            window.Close();
            MainRoot.windowEntrance = null;
        }

        public void SwitchAnotherWindon(object sender, RoutedEventArgs e, Window currentWindow, Window newWindow)
        {
            currentWindow.Close();
            MainRoot.windowEntrance = newWindow;
            MainRoot.windowEntrance.Show();
        }

    }
}
