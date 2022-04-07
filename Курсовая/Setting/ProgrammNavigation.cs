using System.Windows;
using Курсовая.ProgrammInterface;

namespace Курсовая.Setting
{
    internal class ProgrammNavigation : INavigation
    {
        public NotificationWindow notificationWindow;

        public void Cancellation(Window window)
        {
            window.Close();
            MainRoot.windowEntrance = null;
        }

        public void Display(string messange)
        {
            if (notificationWindow != null)
                notificationWindow.Close();
            notificationWindow = new NotificationWindow(messange);
            notificationWindow.ShowDialog();
        }

        public void SwitchAnotherWindon(Window currentWindow, Window newWindow)
        {
            currentWindow.Close();
            MainRoot.windowEntrance = newWindow;
            MainRoot.windowEntrance.Show();
        }
    }
}
