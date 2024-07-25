namespace Rosevale
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string mainMail, string email, string subject, string message);
    }
}
