using System;
using System.Windows.Controls;
using System.Windows.Input;
using Курсовая.ProgrammInterface;

namespace Курсовая.Setting
{
    internal class DataProcessing : IDataProcessing
    {
        public bool SatisfactionRulesPassword(string password, string secondPassword)
        {
            int capitalLatter = default, smallLatter = default, number = default;
            if (password.Length >= 8 && password == secondPassword)
                for (int i = 0; i < password.Length; i++)
                {
                    if (password[i] >= 65 && password[i] <= 90)
                        capitalLatter = 1;
                    else if (password[i] >= 97 && password[i] <= 122)
                        smallLatter = 1;
                    else if (password[i] >= 48 && password[i] <= 57)
                        number = 1;
                }
            return capitalLatter > 0 && smallLatter > 0 && number > 0;
        }
        public void PasswordProcessing(object sender, TextCompositionEventArgs e)
        {
            if ((Convert.ToChar(e.Text) >= (char)48 && Convert.ToChar(e.Text) <= (char)57) || (Convert.ToChar(e.Text) >= (char)97 && Convert.ToChar(e.Text) <= (char)122) || (Convert.ToChar(e.Text) >= (char)65 && Convert.ToChar(e.Text) <= (char)90))
                e.Handled = false;
            else e.Handled = true;
        }
        public void PhoneNumberProcessing(object sender, TextCompositionEventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Trim();
            ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            if (((TextBox)sender).Text.Length == 0 && Convert.ToChar(e.Text) == (char)43)
                e.Handled = false;
            else if ((Convert.ToChar(e.Text) >= (char)48 && Convert.ToChar(e.Text) <= (char)57) && ((TextBox)sender).Text.Length > 0)
                e.Handled = false;
            else e.Handled = true;
        }
        public void LattersProcessing(object sender, TextCompositionEventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Trim();
            ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            if (((TextBox)sender).Text.Length == 0 && ((Convert.ToChar(e.Text) >= (char)97 && Convert.ToChar(e.Text) <= (char)122) || (Convert.ToChar(e.Text) >= (char)65 && Convert.ToChar(e.Text) <= (char)90) || (Convert.ToChar(e.Text) >= (char)1072 && Convert.ToChar(e.Text) <= (char)1103) || (Convert.ToChar(e.Text) >= (char)1040 && Convert.ToChar(e.Text) <= (char)1071)))
            {
                ((TextBox)sender).Text = e.Text.ToUpper();
                ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
                e.Handled = true;
            }
            else if ((Convert.ToChar(e.Text) >= (char)65 && Convert.ToChar(e.Text) <= (char)90) || (Convert.ToChar(e.Text) >= (char)97 && Convert.ToChar(e.Text) <= (char)122) || (Convert.ToChar(e.Text) >= (char)1072 && Convert.ToChar(e.Text) <= (char)1103) || (Convert.ToChar(e.Text) >= (char)1040 && Convert.ToChar(e.Text) <= (char)1071))
                e.Handled = false;
            else e.Handled = true;
        }

        public void EmailTextInput(object sender, TextCompositionEventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Trim();
            ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            if ((Convert.ToChar(e.Text) >= (char)48 && Convert.ToChar(e.Text) <= (char)57) || (Convert.ToChar(e.Text) >= (char)65 && Convert.ToChar(e.Text) <= (char)90) || (Convert.ToChar(e.Text) >= (char)97 && Convert.ToChar(e.Text) <= (char)122) || Convert.ToChar(e.Text) == 46)
                e.Handled = false;
            else e.Handled = true;
        }
        public void EmailTextInputFull(object sender, TextCompositionEventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Trim();
            ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            if ((Convert.ToChar(e.Text) >= (char)48 && Convert.ToChar(e.Text) <= (char)57) || (Convert.ToChar(e.Text) >= (char)65 && Convert.ToChar(e.Text) <= (char)90) || (Convert.ToChar(e.Text) >= (char)97 && Convert.ToChar(e.Text) <= (char)122) || Convert.ToChar(e.Text) == (char)64 || Convert.ToChar(e.Text) == 46)
                e.Handled = false;
            else e.Handled = true;
        }
        public void SymbolProcessing(object sender, TextCompositionEventArgs e)
        {
            if ((Convert.ToChar(e.Text) >= (char)97 && Convert.ToChar(e.Text) <= (char)122) || (Convert.ToChar(e.Text) >= (char)65 && Convert.ToChar(e.Text) <= (char)90) || (Convert.ToChar(e.Text) >= (char)48 && Convert.ToChar(e.Text) <= (char)57))
            {
                ((TextBox)sender).Text = e.Text.ToUpper();
                ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            }
            e.Handled = true;
        }

        public void Birthday(object sender, TextCompositionEventArgs e)
        {
            ((DatePicker)sender).Text = ((DatePicker)sender).Text.Trim();
            if ((Convert.ToChar(e.Text) >= (char)48 && Convert.ToChar(e.Text) <= (char)57) || Convert.ToChar(e.Text)==(char)46)
                e.Handled = false;
            else e.Handled = true;
        }

        public void NumberProcessing(object sender, TextCompositionEventArgs e)
        {
            ((TextBox)sender).Text = ((TextBox)sender).Text.Trim();
            if ((Convert.ToChar(e.Text) >= (char)48 && Convert.ToChar(e.Text) <= (char)57))
                e.Handled = false;
            else e.Handled = true;
        }
    }
}
