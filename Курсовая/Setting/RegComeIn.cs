using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Курсовая.ProgrammInterface;

namespace Курсовая.Setting
{
    internal class RegComeIn : IRegComeIn
    {
        public void TextClear(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Clear();
            ((TextBox)sender).Foreground = Brushes.Black;
        }
       
        public void ShowOrHidePassword(TextBox VP, PasswordBox FP, Button SOrHP)
        {
            VP.Text = FP.Password;
            Image image = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            if (VP.Visibility == Visibility.Hidden)
            {
                VP.Visibility = Visibility.Visible;
                FP.Visibility = Visibility.Hidden;
                bitmap.UriSource = new Uri("../Images/invisible.png", UriKind.Relative);
            }
            else
            {
                VP.Visibility = Visibility.Hidden;
                FP.Visibility = Visibility.Visible;
                bitmap.UriSource = new Uri("../Images/view.png", UriKind.Relative);
            }
            bitmap.EndInit();
            image.Source = bitmap;
            SOrHP.Content = image;
        }

        public void ViewChangingPassword(PasswordBox FP, TextBox VP)=>
            FP.Password = VP.Text;
    }
}
