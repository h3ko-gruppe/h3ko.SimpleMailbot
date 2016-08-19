using System.Threading.Tasks;
using h3ko.SimpleMailbot.Web.Config;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using h3ko.SimpleMailbot.Web.Extensions;

namespace h3ko.SimpleMailbot.Web.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpServerConfig _smtpSetings;

        public SmtpEmailService(IOptions<SmtpServerConfig> smtpSetings)
        {
            _smtpSetings = smtpSetings.Value;
        }

        public async Task SendMail(MailboxAddress[] from, string subject, string body, MailboxAddress[] to, MailboxAddress[] cc = null, MailboxAddress[] bcc = null, bool isHtmlBody = false)
        {
            
            to = to ?? new MailboxAddress[0];
            cc = cc ?? new MailboxAddress[0];
            bcc = bcc ?? new MailboxAddress[0];

            var fromEmail = from ?? _smtpSetings.DefaultFrom.ToMailBoxAddresses();
            
            if (from == null || from.Length == 0)
                throw new NotSupportedException("The email needs at least one sender");

            if (to == null || to.Length == 0)
                throw new NotSupportedException("The email needs at least one recipient");

            var message = new MimeMessage ();
            message.From.AddRange (from);
            message.To.AddRange (to);
            message.Cc.AddRange(cc);
            message.Bcc.AddRange(bcc);
            message.Subject = subject;

            var textPartType = isHtmlBody ? "html" : "plain";
            message.Body = new TextPart (textPartType) 
            {
                Text = body
            };

            if (_smtpSetings.IsEnabled){
                using (var client = new SmtpClient ()) {
                    await client.ConnectAsync (_smtpSetings.Host, _smtpSetings.Port, SecureSocketOptions.Auto);
                    await client.AuthenticateAsync (_smtpSetings.Username, _smtpSetings.Password);
                    await client.SendAsync (message);
                    await client.DisconnectAsync (true);
                }
            }
        }
    }
}
