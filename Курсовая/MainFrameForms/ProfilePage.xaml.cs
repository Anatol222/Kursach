using ProfileClassLibrary;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Курсовая.ProgrammInterface;
using Курсовая.Setting;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Data.SqlClient;
using Курсовая.MainFrameForms;

namespace Курсовая
{

    public partial class ProfilePage : Page
    {
        private DataBase dataBase;
        private Window changePassword;
        private Window confirmEmailWindow;

        private INavigation navigation;
        private IDataProcessing dataProcessing;
        private IDataBaseUserDataVerification userDataVerification;

        private delegate void InvoceMessageBox(string messange);
        private event InvoceMessageBox Notification;

        private delegate void InvoceWarningBox(string content,string buttonText);
        private event InvoceWarningBox Warning;

        private List<TextBox> _textBoxList;
        private List<Button> _buttonList;
        private List<string> _iconAnimals;
        private int _switchIcon = 0, whatGender=0;
        public static bool IsSaveNewData { get; set; }
        public static bool IsEmptyFields { get; set; }

        public ProfilePage()
        {
            InitializeComponent();

            dataBase = new DataBase();
            navigation = new ProgrammNavigation();
            dataProcessing = new DataProcessing();
            userDataVerification = new UserDataVerification();

            //InfoAboutUser();
            //WorkingWithData();

            //Notification += navigation.Display;
            //Warning += userDataVerification.Display;

            //IsEmptyFields = true;
            
            //BirthdayBox.DisplayDateEnd = DateTime.Now.AddYears(-14);
            //BirthdayBox.DisplayDateStart = new DateTime(1920, 01, 01);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (_textBoxList[0].IsReadOnly)
                EditAccess(true, "../Images/ProfileIcon/EditFalse.png");
            else if (!_textBoxList[0].IsReadOnly)
            {
                //ChangeData();
                if (!IsEmptyFields)
                {
                    InfoAboutUser();
                    EditAccess(false, "../Images/ProfileIcon/EditTrue.png");
                }

            }
        }
        public void ChangeIcon(string linkIcon, Button button)
        {
            Image image = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(linkIcon, UriKind.Relative);
            bitmap.EndInit();
            image.Source = bitmap;
            button.Content = image;

        }
        private void ChoiceIcon_Click(object sender, RoutedEventArgs e)
        {
            _switchIcon++;
            if (_switchIcon == _iconAnimals.Count)
                _switchIcon = 0;
            ChangeIcon($"../Images/iconAnimals/{_iconAnimals[_switchIcon]}", ChoiceIcon);
            List<IconUser> list = new List<IconUser>()
            {
                new IconUser($"../Images/iconAnimals/{_iconAnimals[_switchIcon]}")
            };
            File.WriteAllText("UserIcon.json", JsonConvert.SerializeObject(list));
        }

        private void EditAccess(bool access, string linkIcon)
        {
            foreach (TextBox item in _textBoxList)
                item.IsReadOnly = !access;
            foreach (Button item in _buttonList)
                item.IsEnabled = access;
            BirthdayBox.IsEnabled = access;
            ChangeIcon(linkIcon, Edit);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            ChangeIcon("../Images/ProfileIcon/ExitIn.png", Exit);
            if (ConfirmComeIn())
                ExitAccount();
            ChangeIcon("../Images/ProfileIcon/ExitOut.png", Exit);

        }
        private void ExitAccount()
        {
            StartWindow startWindow = new StartWindow();
            startWindow.Show();
            File.Delete("saveUser.json");
            MainFrame.mainFrame.Close();
        }

        private void Wallet_Click(object sender, RoutedEventArgs e)
        {
            ChangeIcon("../Images/ProfileIcon/WalletOpen.png", Wallet);
            if (ConfirmComeIn())
                Notification?.Invoke("Скоро добавим сохранение способа оплаты");
            ChangeIcon("../Images/ProfileIcon/Wallet.png", Wallet);
        }


        private void HotkeysInfo_Click(object sender, RoutedEventArgs e)
        {
            ChangeIcon("../Images/ProfileIcon/Hotkeys2.png", HotkeysInfo);
            if (ConfirmComeIn())
                NavigationService.Navigate(new HotkeysPage());
            ChangeIcon("../Images/ProfileIcon/Hotkeys.png", HotkeysInfo);

        }
        private void InfoAboutUser()
        {
            LastNameBox.Text = MainFrame.user.LastName;
            FirstNameBox.Text = MainFrame.user.Name;
            PatronymicBox.Text = MainFrame.user.Patronymic;
            NumberBox.Text = MainFrame.user.Phone;
            BirthdayBox.Text = Convert.ToString(MainFrame.user.BirthDate.ToShortDateString());
            EmailBox.Text = MainFrame.user.Email;
            ReserveEmail.Text = MainFrame.user.ReserveEmail;
            if (ReserveEmail.Text == "")
                ReserveEmail.Text = "Резервная почта не указана";
            EmailUres.Content = MainFrame.user.Email;
            FNamePatronymic.Content = MainFrame.user.Name + " " + MainFrame.user.Patronymic;
            Gender gender = MainFrame.user.Gender;
            if (gender == Gender.Male)
            { GenderColor(Brushes.White, Brushes.Aqua); whatGender = 0; }
            else if (gender == Gender.Female)
            { GenderColor(Brushes.RosyBrown, Brushes.White); whatGender = 1; }
        }

        private void MaleBtn_Click(object sender, RoutedEventArgs e) =>
            GenderColor(Brushes.White, Brushes.Aqua);
        private void FemaleBtn_Click(object sender, RoutedEventArgs e) =>
            GenderColor(Brushes.RosyBrown, Brushes.White);

        private void GenderColor(SolidColorBrush female, SolidColorBrush male)
        {
            FemaleBtn.Background = female;
            MaleBtn.Background = male;
            if (female == Brushes.White)
                whatGender = 0;
            else whatGender = 1; 

        }

        private bool ConfirmComeIn()
        {
            if (_buttonList[0].IsEnabled == true)
            {
                Warning?.Invoke("Вы уверены, что хотите сохранить данные изменения?", "Подтвердить");
                if (IsSaveNewData)
                {
                    ChangeData();
                    if (!IsEmptyFields)
                    {
                        InfoAboutUser();
                        EditAccess(false, "../Images/ProfileIcon/EditTrue.png");
                    }
                }
                else
                {
                    EditAccess(false, "../Images/ProfileIcon/EditTrue.png");
                    InfoAboutUser();
                }

            }
            return !_buttonList[0].IsEnabled;
        }

        private void ChangeData()
        {
            bool allFieldsFill = true;
            foreach (TextBox item in _textBoxList)
                if (item.Text.Trim() == "")
                { allFieldsFill = false; break; }
            if (allFieldsFill)
            {
                if (!userDataVerification.Verification($"SELECT * FROM UserPersonalData WHERE Number ='{NumberBox.Text.Trim()}';") || NumberBox.Text.Trim() == MainFrame.user.Phone)
                {
                    if (!userDataVerification.Verification($"SELECT * FROM PersonalLoginData WHERE Email = '{EmailBox.Text.Trim()}'") || EmailBox.Text.Trim() == MainFrame.user.Email)
                    {
                        if (!userDataVerification.Verification($"SELECT * FROM PersonalLoginData WHERE Email='{ReserveEmail.Text.Trim()}' OR ReserveEmail = '{ReserveEmail.Text.Trim()}'"))
                        {
                            try
                            {
                                string query = $"UPDATE UserPersonalData SET FirstName='{FirstNameBox.Text.Trim()}',LastName='{LastNameBox.Text.Trim()}',Number='{NumberBox.Text.Trim()}'," +
                                    $"Patronymic='{PatronymicBox.Text.Trim()}',Birthday='{Convert.ToDateTime(BirthdayBox.Text.Trim()).ToString(@"MM/dd/yyyy")}',Gender ={whatGender} WHERE Number='{MainFrame.user.Phone}';";
                                SqlCommand sqlCommand = new SqlCommand(query, dataBase.GetConnection());
                                dataBase.OpenConnection();
                                if (sqlCommand.ExecuteNonQuery() == 1)
                                {
                                    if (ReserveEmail.Text.Trim() != "Резервная почта не указана" && ReserveEmail.Text.Trim() != "" && ReserveEmail.Text.Trim() != MainFrame.user.ReserveEmail)
                                    {
                                        query = $"UPDATE PersonalLoginData SET ReserveEmail = '{ReserveEmail.Text.Trim()}' WHERE Email = '{MainFrame.user.Email}'; ";
                                        sqlCommand = new SqlCommand(query, dataBase.GetConnection());
                                        if (sqlCommand.ExecuteNonQuery() != 1)
                                            Notification?.Invoke("Не удается обновить резервную почту");
                                    }
                                    if (EmailBox.Text.Trim() != MainFrame.user.Email && EmailBox.Text.Trim() != "")
                                    {
                                        confirmEmailWindow = new ConfirmEmailWinow(EmailBox.Text.Trim());
                                        confirmEmailWindow.ShowDialog();
                                        if (!ConfirmEmailWinow.IsReplecement)
                                            EmailBox.Text = MainFrame.user.Email;
                                        ConfirmEmailWinow.IsReplecement = false;
                                    }
                                    IsEmptyFields = true;
                                    EditAccess(false, "../Images/ProfileIcon/EditTrue.png");
                                    MainFrame.user = new User(EmailBox.Text);
                                    dataBase.CloseConnection();
                                    InfoAboutUser();

                                }
                                else
                                    Notification?.Invoke("Не удается обновить личные данные");
                            }
                            catch
                            {
                                Notification?.Invoke("Ошибка при обновлении данных");
                            }
                        }
                        else
                            Notification?.Invoke("Пользователь с такой почтой уже существует. Вы не можете изменить резервную почту");
                    }
                    else
                        Notification?.Invoke("Пользователь с такой почтой уже существует. Вы не можете изменить почту");
                }
                else
                    Notification?.Invoke("Пользователь с таким номером уже существует. Вы не можете изменить номер");
            }
            else
                Warning?.Invoke("Не все поля заполнены. Нажмите продолжить, если не хотите ничего менять", "Продолжить");
        }

        private void PhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)=>
            dataProcessing.PhoneNumberProcessing(sender, e);
        private void Latters_PreviewTextInput(object sender, TextCompositionEventArgs e)=>
            dataProcessing.LattersProcessing(sender, e);
        private void Email_PreviewTextInput(object sender, TextCompositionEventArgs e)=>
            dataProcessing.EmailTextInputFull(sender, e);
        private void Birthday_PreviewTextInput(object sender, TextCompositionEventArgs e)=>
            dataProcessing.Birthday(sender, e);

        private void WorkingWithData()
        {
            _textBoxList = new List<TextBox>()
            {
                LastNameBox,FirstNameBox,PatronymicBox,NumberBox,EmailBox,ReserveEmail,
            };
            _buttonList = new List<Button>()
            {
                MaleBtn,FemaleBtn,ButtonChangePassword
            };
            _iconAnimals = new List<string>()
            {
                "ape.png","bird.png","bull.png","camel.png","cat.png","cow.png","dog.png","duck.png","elephant.png","giraffe.png","goose.png","green_love.png","hors.png","lion.png",
                "mouse.png","nature.png","owl.png","panda.png","tartaruga.png","tiger.png","wolf.png"
            };
            if (File.Exists("UserIcon.json"))
            {
                DataContractJsonSerializer jsonF = new DataContractJsonSerializer(typeof(List<IconUser>));
                List<IconUser> dataUserSave = new List<IconUser>();
                using (FileStream fs = new FileStream("UserIcon.json", FileMode.Open))
                    dataUserSave = (List<IconUser>)jsonF.ReadObject(fs);
                ChangeIcon(dataUserSave[0].IconAn, ChoiceIcon);
            }
        }

        private void ToChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmComeIn())
            {
                changePassword = new ChangePassword();
                changePassword.ShowDialog();
                if (ChangePassword.IsChangePasswor)
                    ExitAccount();
            }
        }

        
    }
}
