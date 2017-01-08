using System.Reflection;
using Microsoft.AspNetCore.Mvc;

[assembly:AssemblyCompany("h3ko GmbH")]
[assembly:AssemblyCopyrightAttribute("2016 © Rico Herlt")]

namespace h3ko.SimpleMailbot.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        
        public IActionResult Error()
        {
            return View();
        }
    }
}
