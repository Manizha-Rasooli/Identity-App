using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IdentityApp.Helper
{
    public class PasswordReset
    {
        public static void PasswordResetSendEmail(string link,string email)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("your email");
            mail.To.Add(email);

            mail.Subject = $"www.mypage.com::Şifre Sıfırlama";
            mail.Body = "<h2>şifrenizi yenilemek için lütfen aşağıdaki linki tıklayınız</h2><hr/>";
            mail.Body = $"a href='{link}'>şifre yenileme linki</a>";
            mail.IsBodyHtml = true;
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new System.Net.NetworkCredential("your email", "password");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
          
        }
    }
}
