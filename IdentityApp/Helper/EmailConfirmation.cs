using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IdentityApp.Helper
{
    public static class EmailConfirmation
    {
        public static void SendEmail(string link, string email)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("manizharasooli28@gmail.com");
            mail.To.Add(email);

            mail.Subject = $"www.mypage.com::E-Posta doğrulama";
            mail.Body = "<h2>E-Posta adresinizi doğrulamak için lütfen aşağıdaki linki tıklayınız</h2><hr/>";
            mail.Body = $"a href='{link}'>email doğrulama linki</a>";
            mail.IsBodyHtml = true;
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new System.Net.NetworkCredential("manizharasooli28@gmail.com", "01A06m0550");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }

        }
    }
}
