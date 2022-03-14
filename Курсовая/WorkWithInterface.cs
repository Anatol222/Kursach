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
        private string _notifyAboutCodeEmail = "       Мы отправили 5-значный код на Email." +Environment.NewLine+
            "Подтвердите, что почта принадлежит вам, чтобы"+ Environment.NewLine +"         обезопасить свою учётную запись";
        public string NotifyAboutCodeEmail { get { return _notifyAboutCodeEmail; } }

        private string _notifyAboutReserveEmail = " Укажите резервную почту. Она поможет восстановить доступ, " + Environment.NewLine +
            "если не получится войти в аккаунт. Также на эту почту будут " + Environment.NewLine+
            "приходить уведомления о действиях, связанных с безопасностью аккаунта";
        public string NotificationAboutReserveEmail { get { return _notifyAboutReserveEmail; } }

        private string _notificationPassword = "Пароль должен содержать:" + Environment.NewLine + " • Минимум 8 элементов"
                + Environment.NewLine + " • Одну большую букву" + Environment.NewLine + " • Одну маленькую букву"
                + Environment.NewLine + " • Одну цифру" + Environment.NewLine + " • И содержать в себе символы (A,z,1,_,-)";
        public string NotificationPassword { get { return _notificationPassword; } }

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

        public bool PasswordProcessing(string password, string secondPassword)
        {
            int capitalLatter = default, smallLatter = default, number = default;
            if (password.Length>=8 && password==secondPassword)
                for (int i = 0; i < password.Length; i++)
                {
                    if (password[i]>=65 &&password[i]<=90)
                        capitalLatter = 1;
                    else if (password[i] >= 97 && password[i] <= 122)
                        smallLatter = 1;
                    else if (password[i] >= 48 && password[i] <= 57)
                        number = 1;
                }
            return capitalLatter>0 && smallLatter>0 && number>0;
        }

    }
}
