using System.Windows;

namespace Курсовая.ProgrammInterface
{
    internal interface INavigation
    {
        void Display(string messange);

        void Cancellation(Window window);

        void SwitchAnotherWindon(Window currentWindow, Window newWindow);
    }
}
