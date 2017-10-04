using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.Extensions.Logging;

namespace WebAppCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


      
            public HomeController(ILogger<HomeController> logger)
            {
                _logger = logger;
            }
        
   //    [ResponseCache(Duration = 100)]

        public IActionResult Index()
        {
            if (_logger != null) throw new NullReferenceException();

            return Content("123");
           /* throw new NullReferenceException();
            return View(); */
        }

        public IActionResult About()
        {
            _logger.LogInformation("Index page says hello");
            _logger.LogDebug("Index debug page says hello");
            _logger.LogTrace("Index trace page says hello");


            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
