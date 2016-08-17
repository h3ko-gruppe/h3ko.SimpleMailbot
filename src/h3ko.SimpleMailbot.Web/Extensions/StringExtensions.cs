using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;

namespace h3ko.SimpleMailbot.Web.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a Secure string from the source string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static SecureString ToSecureString(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;
            else
            {
                SecureString Result = new SecureString();
                foreach (char c in source.ToCharArray())
                    Result.AppendChar(c);
                return Result;
            }
        }

        public static string Decrypt(this SecureString value)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(value);

            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }

        public static MailAddress[] ToMailAddresses(this string val)
        {
            if (string.IsNullOrEmpty(val) || val.EndsWith(" "))
                return new MailAddress[0];
            else
            {
                var seperators = new[] { ',', ';' };
                var result = val.Split(seperators, StringSplitOptions.RemoveEmptyEntries).AsEnumerable().Select(x => new MailAddress(x)).ToArray();
                return result;
            }
        }
    }
}
