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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;

namespace WebAppCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _appDbContext;
        private readonly ContextCheckingService _ccs;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IStringLocalizer<SharedResource> _resourceLocalizer;

        public HomeController(ILogger<HomeController> logger, 
            ApplicationDbContext appDbContext , 
            ContextCheckingService ccs, 
            IStringLocalizer<HomeController> localizer,
            IStringLocalizer<SharedResource> resourceLocalizer
            )
        {
            _resourceLocalizer = resourceLocalizer;
            _localizer = localizer;
            _logger = logger;
            _appDbContext = appDbContext;
            _ccs = ccs;
           

        }

        //    [ResponseCache(Duration = 100)]

        public IActionResult Index()
        {

            
            
           var feature= HttpContext.Features.Get<IRequestCultureFeature>();
            ViewBag.Content = $"Culture is : {feature.RequestCulture.UICulture}  ,{_localizer["String2"]} { _resourceLocalizer["String1"]}";
            return View();
        }
        [Authorize(Roles="Manager")]
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
