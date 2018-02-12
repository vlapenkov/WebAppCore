using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.Extensions.Logging;
using WebAppCore.Data;
using WebAppCore.Services;

namespace WebAppCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _appDbContext;
        private readonly ContextCheckingService _ccs;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext appDbContext , ContextCheckingService ccs)
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _ccs = ccs;

        }

        //    [ResponseCache(Duration = 100)]

        public IActionResult Index()
        {


            /*   return Content("dev1");
              /* throw new NullReferenceException(); */
            return View();
        }

        public IActionResult About()
        {
            _logger.LogInformation("dev1");
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

        public IActionResult Products(int id)
        {
            var product = _appDbContext.Products.FirstOrDefault(p => p.Id == id);
            //return Json(new { id = product.Id, name = product.Name });
            return Json(product);
        }

        public IActionResult GetContext()=>
        
           Json(_ccs.GetField("first"));
        
    }
}
