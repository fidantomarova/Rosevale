using System.Net;
using System.Net.Mail;

namespace Rosevale
{
    public class EmailSender: IEmailSender
    {
        internal static async Task SendEmailAsync(string email, string v1, string v2)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailAsync(string mainMail, string email, string subject, string message)
        {
            string password = "zvcoknmwvcciiimg";
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mainMail, password)
            };
            return client.SendMailAsync(new MailMessage(from: mainMail, to: email, subject, message));
        }
    }
}
