using System;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;

namespace ConsoleFree
{
    public class Email
    {
        private List<string> _emails = new List<string>()
        {
            "@mail.ru",
            "@internet.ru",
            "@bk.ru",
            "@inbox.ru",
            "@list.ru",
            "@gmail.com"
        };
        public List<string> Emails { get { return _emails; } }
        public void SendMessageConfirmationEmail(string code, string email)
        {
            try
            {
                MailAddress from = new MailAddress("kirill10.01.2003@mail.ru");
                MailAddress to = new MailAddress(email);
                MailMessage message = new MailMessage(from, to);
                message.Subject = "Подтверждение электронной почты для регистрации";
                message.Body = $"<h2>{code}</h2>";
                message.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.mail.ru");
                smtp.Credentials = new NetworkCredential("kirill10.01.2003@mail.ru", "p08KRZ0HyusCFfM1y0ig");
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
            catch (Exception)
            {
                //добавить
            }
            

        }
        //tolya.bob@inbox.ru
        public void SendMessageNewPassword(string password,string email)
        {
            MailAddress from = new MailAddress("kirill10.01.2003@mail.ru");
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Сброс пароля учётной записи";
            message.Body = $"<h2>{password}</h2>";
            message.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.mail.ru");
            smtp.Credentials = new NetworkCredential("kirill10.01.2003@mail.ru", "p08KRZ0HyusCFfM1y0ig");
            smtp.EnableSsl = true;
            smtp.Send(message);
        }
        public string CreatingPassword()
        {
            string password = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 8; i++)
            {
                if (random.Next(0, 2) == 1)
                    password += (char)random.Next(65, 90);
                else
                    password += (char)random.Next(49, 57);
            }
            return password;
        }
        public string CreatingCodeConfirmation()
        {
            string code = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                if (random.Next(0, 2) == 1)
                    code += (char)random.Next(65, 90);
                else
                    code += (char)random.Next(49, 57);
            }
            return code;
        }
    }
}
