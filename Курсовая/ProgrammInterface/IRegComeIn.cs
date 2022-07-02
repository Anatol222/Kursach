using System.Windows;
using System.Windows.Controls;

namespace Курсовая.ProgrammInterface
{
    internal interface IRegComeIn
    {
        void TextClear(object sender, RoutedEventArgs e);

        void ShowOrHidePassword(TextBox VP, PasswordBox FP, Button SOrHP);

        void ViewChangingPassword(PasswordBox FP, TextBox VP);
    }
}
