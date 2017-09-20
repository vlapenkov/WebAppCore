using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace WebAppCore.Controllers
{
    public class SettingsManageController : Controller
    {

        private readonly MySettings _settings;

        public SettingsManageController(IOptions<MySettings> optionsAccessor)
        {
            _settings = optionsAccessor.Value;


        }

        public IActionResult Index()
        {
            return Content($"option1 = {_settings.ApplicationName}, option2 = {_settings.MaxItemsPerList}");
        }

        public IActionResult About([FromServices] IOptions<MySettings> optionsAccessor, [FromServices] IConfiguration configuration)
        {
            var myKey = configuration.GetValue<string>("MyKey");
            var myKey2 = configuration.GetValue<IEnumerable<int>>("MyKey2");

            return Content($"Appname = {optionsAccessor.Value.ApplicationName} max items= {optionsAccessor.Value.MaxItemsPerList} myKey= {myKey}");

            //  return View();
        }

        public IActionResult Simple([FromServices]  IConfiguration configuration)
        {
            MySettings set = (MySettings)configuration.GetSection("MySettings");
            return Content($"Appname = {set.ApplicationName}");
        }
    }
    }