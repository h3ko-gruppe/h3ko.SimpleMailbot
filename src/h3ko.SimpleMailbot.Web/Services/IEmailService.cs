using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace h3ko.SimpleMailbot.Web.Services
{
    public interface IEmailService
    {
        Task SendMail(MailAddress from, string subject, string body, MailAddress[] to, MailAddress[] cc = null, MailAddress[] bcc = null, bool isHtmlBody = false);
    }
}