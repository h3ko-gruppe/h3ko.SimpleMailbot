using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel.Configuration;
using System.Threading.Tasks;
using h3ko.SimpleMailbot.Web.Config;
using h3ko.SimpleMailbot.Web.Extensions;
using h3ko.SimpleMailbot.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.v3;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace h3ko.SimpleMailbot.Web.Controllers
{
    [Route("api/[controller]")]
    public class SmtpController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly List<TokenEmailConfig> _tokenEmailConfigs;
        private const string PostEmail = "PostEmail";

        public SmtpController(IEmailService emailService, IOptions<List<TokenEmailConfig>> tokenEmailConfigs)
        {
            _emailService = emailService;
            _tokenEmailConfigs = tokenEmailConfigs.Value;
        }

        // GET: api/smtp
        [HttpGet]
        public IActionResult Get()
        {
            const string caption = "Simple Mailbot chilling here...";
            const string info = "Hey guys, whats up? I am just hanging around here. Please POST me some mails to forward for you ;). <br>I show you how to do. Make sure to send somehow this HTTP POST request:";

            var url = Url.Link(PostEmail, new {from = "sender@mail.com", to = "reciepant@mail.com", subject = "My Awesome e-mail!", cc = "carboncopy@mail.com", bcc = "blindcarbonCopy@mail.com", isHtmlBody=true });
            var host = Url.ActionContext.HttpContext.Request.Host;

            var httpPost =
                "POST " + url + " HTTP/1.1<br>" +
                "Content-Type: text/plain<br>" +
                "api-token: a8e9685058d248f5a9870f376be0e942<br>" +
                "Host: "+host+"<br>" +
                "Content-Length: 22<br>" +
                "<br>" +
                "This is my e-mail body";

            var html =
                "<html>" +
                "   <head>" +
                "   </head>" +
                "<body>" +
                "   <h1>" + 
                        caption +
                "   </h1>" +
                "   <p>" + info +"</p>" +
                "   <p style=\"background-color:lightgrey;\">" + httpPost +"</p>" +
                "   </body>" +
                "</html>";
            Response.ContentType = "text/html";
            return Ok(html);
        }

        // POST api/smtp
        [HttpPost(Name=PostEmail)]
        public async Task<IActionResult> Post([FromQuery]string from, [FromQuery]string to, [FromQuery]string cc, [FromQuery]string bcc, [FromHeader(Name = "api-token")]string headerToken, [FromQuery(Name = "api-token")]string queryToken, [FromQuery]string subject, [FromBody]string body, bool isHtmlBody)
        {
            MailAddress fromMail;
            MailAddress[] tos;
            MailAddress[] ccs;
            MailAddress[] bccs;
            try
            {
                var token = !string.IsNullOrEmpty(headerToken) ? headerToken : queryToken;
                if (string.IsNullOrEmpty(token))
                    return BadRequest("No api-token found in HTTP-Header or in url query string");


                var currentTokenconfiguration = _tokenEmailConfigs.FirstOrDefault(x => x.Token == token);

                if (currentTokenconfiguration == null)
                    return BadRequest($"No matching configuration found for this api-token: {token}");

                //replace arguments with values from configuration
                from = !string.IsNullOrEmpty(currentTokenconfiguration.From) ? currentTokenconfiguration.From : from;
                to = !string.IsNullOrEmpty(currentTokenconfiguration.To) ? currentTokenconfiguration.To : to;
                cc = !string.IsNullOrEmpty(currentTokenconfiguration.Cc) ? currentTokenconfiguration.Cc : cc;
                bcc = !string.IsNullOrEmpty(currentTokenconfiguration.Bcc) ? currentTokenconfiguration.Bcc : bcc;
                subject = !string.IsNullOrEmpty(currentTokenconfiguration.Subject) ? currentTokenconfiguration.Subject : subject;
                body = !string.IsNullOrEmpty(currentTokenconfiguration.Body) ? currentTokenconfiguration.Body : body;
                isHtmlBody = isHtmlBody || Request.ContentType.ToLower().Contains("html");
                
                fromMail = from == null ? null : new MailAddress(from);
                tos = to.ToMailAddresses();
                ccs = cc.ToMailAddresses();
                bccs = bcc.ToMailAddresses();
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            await _emailService.SendMail(fromMail, subject, body, tos, ccs, bccs, isHtmlBody);
            return Ok();
        }

    }
}
