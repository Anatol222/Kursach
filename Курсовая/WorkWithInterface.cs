using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Курсовая
{
    internal class WorkWithInterface:Window
    {
        private string _notifyAboutCodeEmail = "    Мы отправили 5-значный код на Email. Подтвердите, что почта принадлежит вам, чтобы обезопасить свою учётную запись";
        public string NotifyAboutCodeEmail { get { return _notifyAboutCodeEmail; } }

        private string _notifyAboutReserveEmail = " Укажите резервную почту. Она поможет восстановить доступ, если не получится войти в аккаунт. Также на эту почту будут приходить уведомления о действиях, связанных с безопасностью аккаунта";
        public string NotificationAboutReserveEmail { get { return _notifyAboutReserveEmail; } }

        private string _notificationPassword = "Пароль должен содержать:" + Environment.NewLine + " • Минимум 8 элементов"
                + Environment.NewLine + " • Одну большую букву" + Environment.NewLine + " • Одну маленькую букву"
                + Environment.NewLine + " • Одну цифру" + Environment.NewLine + " • И содержать в себе символы (A,z,1,_,-)";
        public string NotificationPassword { get { return _notificationPassword; } }

        private string _policy = " СОГЛАШЕНИЕ ДЛЯ ОБРАБОТКИ ПЕРСОНАЛЬНЫХ ДАННЫХ И ПОЛИТИКА КОНФИДЕНЦИАЛЬНОСТИ"+Environment.NewLine + 
            "   Присоединяясь к настоящему Соглашению и оставляя свои данные в Horizont, путем заполнения полей в Личном кабинете(регистрации), Пользователь:"+Environment.NewLine + 
            "1. Подтверждает, что все указанные им данные принадлежат лично ему;"+Environment.NewLine +
            "2. Подтверждает и признает, что им внимательно в полном объеме прочитано Соглашение и условия обработки его персональных данных, указываемых им в полях регистрации в Личном кабинете(регистрации), текст соглашения и условия обработки персональных данных ему понятны;" + Environment.NewLine +
            "3. Дает согласие на обработку Сайтом предоставляемых в составе информации персональных данных в целях заключения между ним и Сайтом настоящего Соглашения, а также его последующего исполнения;" + Environment.NewLine +
            "4. Выражает согласие с условиями обработки персональных данных без оговорок и ограничений." +Environment.NewLine + 
            "   Пользователь дает свое согласие на обработку его персональных данных, а именно совершение действий, предусмотренных <О персональных данных>, и подтверждает, что, давая такое согласие, он действует свободно, своей волей и в своем интересе."+Environment.NewLine + 
            "   Согласие Пользователя на обработку персональных данных является конкретным, информированным и сознательным."+Environment.NewLine + 
            "   Настоящее согласие Пользователя признается исполненным в простой письменной форме, на обработку следующих персональных данных: фамилии, имени, отчества; места пребывания(город, область); номерах телефонов; адресах электронной почты(E-mail)."+Environment.NewLine + 
            "   Пользователь, предоставляет Horizon право осуществлять следующие действия(операции) с персональными данными: сбор и накопление; хранение в течение установленных нормативными документами сроков хранения отчетности, но не менее года, с момента даты прекращения пользования услуг Пользователем; уточнение(обновление, изменение); использование; уничтожение; обезличивание; передача по требованию суда, в т.ч., третьим лицам, с соблюдением мер, обеспечивающих защиту персональных данных от несанкционированного доступа."+Environment.NewLine + 
            "   Указанное согласие действует бессрочно с момента предоставления данных и может быть отозвано Пользователем путем подачи заявления администрации сайта с указанием данных, определенных Закона «О персональных данных»."+Environment.NewLine + 
            "   Отзыв согласия на обработку персональных данных может быть осуществлен путем направления Пользователем соответствующего распоряжения в простой письменной форме на адрес электронной почты (E-mail) kirill10.01.2003@mail.ru"+Environment.NewLine + 
            "   Horizon имеет право вносить изменения в настоящее Соглашение.При внесении изменений в актуальной редакции указывается дата последнего обновления.Новая редакция Соглашения вступает в силу с момента ее размещения, если иное не предусмотрено новой редакцией Соглашения.";
        public string Policy { get { return _policy; } }

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
        public void PhoneNumberProcessing(object sender, TextCompositionEventArgs e)
        {
            ((TextBox)sender).Text=((TextBox)sender).Text.Trim();
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
            if (((TextBox)sender).Text.Length == 0 && ((Convert.ToChar(e.Text) >= (char)97 && Convert.ToChar(e.Text) <= (char)122) || (Convert.ToChar(e.Text) >= (char)65 && Convert.ToChar(e.Text) <= (char)90)))
            {
                ((TextBox)sender).Text = e.Text.ToUpper();
                ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
                e.Handled = true;
            }
            else if ((Convert.ToChar(e.Text) >= (char)65 && Convert.ToChar(e.Text) <= (char)90) || (Convert.ToChar(e.Text) >= (char)97 && Convert.ToChar(e.Text) <= (char)122))
                e.Handled = false;
            else e.Handled = true;
        }

        public string decode(string str) => encode(str);

        public string encode(string str)
        {
            string text = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= 'a' && str[i] <= 'z')
                    text += Cipher(str[i], 'a', 'z');
                else if (str[i] >= 'A' && str[i] <= 'Z')
                    text += Cipher(str[i], 'A', 'Z');
                else if (str[i] >= '0' && str[i] <= '9')
                    text += Cipher(str[i], '0', '9');
            }
            return text;
        }

        private char Cipher(char letter, int firstLatter, int lastLatter) =>
            (char)(lastLatter - (letter - firstLatter));
    }
}
