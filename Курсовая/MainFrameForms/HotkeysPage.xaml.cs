using System.Windows.Controls;

namespace Курсовая.MainFrameForms
{
    public partial class HotkeysPage : Page
    {
        private TextInterface textInterface;
        public HotkeysPage()
        {
            InitializeComponent();
            textInterface = new TextInterface();
            HotKeysnBox.Text = textInterface.HotkeysInfo;
        }
    }
}
