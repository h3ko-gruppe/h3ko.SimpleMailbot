using MimeKit;
using System.Threading.Tasks;

namespace h3ko.SimpleMailbot.Web.Services
{
    public interface IEmailService
    {
        Task SendMail(MailboxAddress[] from, string subject, string body, MailboxAddress[] to, MailboxAddress[] cc = null, MailboxAddress[] bcc = null, bool isHtmlBody = false);
    }
}