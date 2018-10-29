using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebAppCore.Models;
using WebAppCore.Services;

namespace WebAppCore.Controllers
{
    public class DummyController : Controller
    {
        private readonly IStringLocalizer<DummyController> _localizer;
        private readonly IStringLocalizer<SharedResource> _resourceLocalizer;

        private readonly LocalizationService _localizationService;

        public DummyController(IStringLocalizer<DummyController> localizer, IStringLocalizer<SharedResource> resourceLocalizer, LocalizationService localizationService)
        {
            _localizer = localizer;
            _resourceLocalizer = resourceLocalizer;
            _localizationService = localizationService;
        }

        public IActionResult Index()
        {
            var model = new KendoModel {StartDate=DateTime.Now };
           // return Content(_localizer["String1"]);

            return View(model);
        }

        public IActionResult GetString()
        {

         var val =  _localizationService.GetResource("carts.text1", 1);

            return Content(val);
        }
    }
}