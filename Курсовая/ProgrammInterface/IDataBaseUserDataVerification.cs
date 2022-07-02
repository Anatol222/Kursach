namespace Курсовая.ProgrammInterface
{
    internal interface IDataBaseUserDataVerification
    {
        bool Verification(string query);

        void Display(string conten, string buttonText);
    }
}
