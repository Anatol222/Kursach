using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Курсовая
{
    internal interface IRegistr
    {
        bool ChechUser();
        void StaticRules();
        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e);
        void Email_PreviewTextInput(object sender, TextCompositionEventArgs e);
        void ConfirmEmail_Click(object sender, RoutedEventArgs e);
        void Cancellation_Click(object sender, RoutedEventArgs e);
    }
}
