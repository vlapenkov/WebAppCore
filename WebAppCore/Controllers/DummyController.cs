using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebAppCore.Models;

namespace WebAppCore.Controllers
{
    public class DummyController : Controller
    {
        private readonly IStringLocalizer<DummyController> _localizer;
        private readonly IStringLocalizer<SharedResource> _resourceLocalizer;

        public DummyController(IStringLocalizer<DummyController> localizer, IStringLocalizer<SharedResource> resourceLocalizer)
        {
            _localizer = localizer;
            _resourceLocalizer = resourceLocalizer;
        }

        public IActionResult Index()
        {
            var model = new KendoModel {StartDate=DateTime.Now };
           // return Content(_localizer["String1"]);

            return View(model);
        }
    }
}