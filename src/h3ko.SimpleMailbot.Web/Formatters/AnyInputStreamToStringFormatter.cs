using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace h3ko.SimpleMailbot.Web.Formatters
{
    public class AnyInputStreamToStringFormatter : IInputFormatter
    {
        public bool CanRead(InputFormatterContext context)
        {
            return true;
        }

        public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            return Task.Run(() =>
            {
                using (var reader = new StreamReader(context.HttpContext.Request.Body))
                {
                   var contents = reader.ReadToEnd();
                   return InputFormatterResult.Success(contents);
                }
            });
        }
    }
}
