using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using h3ko.SimpleMailbot.Web.Config;
using Microsoft.Extensions.Options;
using NuGet.Packaging;

namespace h3ko.SimpleMailbot.Web.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpServerConfig _smtpSetings;

        public SmtpEmailService(IOptions<SmtpServerConfig> smtpSetings)
        {
            _smtpSetings = smtpSetings.Value;
        }

        public Task SendMail(MailAddress from, string subject, string body, MailAddress[] to, MailAddress[] cc = null, MailAddress[] bcc = null, bool isHtmlBody = false)
        {
            var fromEmail = from ?? new MailAddress(_smtpSetings.DefaultFrom); ;
            var mail = new MailMessage
            {
                Subject = subject,
                Body = body,
                // Attachments = attachments.ToAttachments();
                IsBodyHtml = isHtmlBody,
                From = fromEmail
            };
            mail.To.AddRange(to);
            mail.CC.AddRange(cc);
            mail.Bcc.AddRange(bcc);

            var client = new SmtpClient
            {
                Port = _smtpSetings.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSetings.Username, _smtpSetings.Password),
                EnableSsl = _smtpSetings.UseTls,
                Host = _smtpSetings.Host
            };

            return client.SendMailAsync(mail);
        }
    }
}
