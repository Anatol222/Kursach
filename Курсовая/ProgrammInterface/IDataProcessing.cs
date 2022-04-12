using System.Windows.Input;

namespace Курсовая.ProgrammInterface
{
    internal interface IDataProcessing
    {
        void LattersProcessing(object sender, TextCompositionEventArgs e);
        void PhoneNumberProcessing(object sender, TextCompositionEventArgs e);
        bool SatisfactionRulesPassword(string password, string secondPassword);
        void PasswordProcessing(object sender, TextCompositionEventArgs e);
        void EmailTextInput(object sender, TextCompositionEventArgs e);
        void EmailTextInputFull(object sender, TextCompositionEventArgs e);
        void SymbolProcessing(object sender, TextCompositionEventArgs e);
        void Birthday(object sender, TextCompositionEventArgs e);
    }
}
