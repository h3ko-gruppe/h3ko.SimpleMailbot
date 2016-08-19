using System;
using System.Linq;
using MimeKit;

namespace h3ko.SimpleMailbot.Web.Extensions
{
    public static class StringExtensions
    {
        public static MailboxAddress[] ToMailBoxAddresses(this string val)
        {
            if (string.IsNullOrEmpty(val) || val.EndsWith(" "))
                return new MailboxAddress[0];
            else
            {
                var multipleEmailsInOneStringSeperators = new[] { ',', ';' };
                var result = val.Split(multipleEmailsInOneStringSeperators, StringSplitOptions.RemoveEmptyEntries).Select(x => 
                {
                    var emailSeperators = new[] { '<', '>', '(', ')', '[', ']', '{', '}' };
                    var mailAddressPart = x.Split(emailSeperators,StringSplitOptions.RemoveEmptyEntries);
                    return new MailboxAddress(mailAddressPart[0], mailAddressPart[1]);
                }).ToArray();
                return result;
            }
        }
    }
}
