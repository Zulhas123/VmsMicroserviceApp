using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Base.API.Manager
{
    public static class Utility
    {
     
        public static void SendMail(string to, string subject, string body, string sender, string password)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(sender);
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };
            // send email
            using (var smtp = new SmtpClient())
            {

                smtp.Connect("smtp.gmail.com", 587, false);
                //normal password Intertechbd.com@vms
                //using app password from vms - 16 digit pass is > ztzjctzfwvavesxn
                smtp.Authenticate(sender, password);
                smtp.Send(email);
                smtp.Disconnect(true);
            };
        }
    }
}
