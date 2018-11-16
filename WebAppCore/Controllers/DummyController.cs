using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using WebApp.DAL;
using WebApp.Services;
using WebAppCore.Models;
using WebAppCore.Services;

namespace WebAppCore.Controllers
{
    public class DummyController : Controller
    {
        private readonly IStringLocalizer<DummyController> _localizer;
        private readonly IStringLocalizer<SharedResource> _resourceLocalizer;

        private readonly LocalizationService _localizationService;
        private readonly JsonLocalizationService _ls;

        public DummyController(IStringLocalizer<DummyController> localizer, IStringLocalizer<SharedResource> resourceLocalizer, 
            LocalizationService localizationService,
            JsonLocalizationService ls)
        {
            _localizer = localizer;
            _resourceLocalizer = resourceLocalizer;
            _localizationService = localizationService;
            _ls = ls;
        }

        public IActionResult Index()
        {
            var model = new KendoModel {StartDate=DateTime.Now };
           // return Content(_localizer["String1"]);

            return View(model);
        }

        public IActionResult IndexEnums()
        {
            OrderType orderType = OrderType.First;
         var val= _ls.GetLocalizedEnum(orderType);

            return Content(val);
        }

        public IActionResult GetString()
        {

         var val =  _localizationService.GetResource("carts.text1", 1);
            var val2 = _ls["text1"];
            return Content($"val is { val}, val2 is {val2}");
        }


        public IActionResult Elastic()
        {

            var settings = new ConnectionConfiguration(new Uri("http://localhost:9200"))
    .RequestTimeout(TimeSpan.FromMinutes(2));

            var lowlevelClient = new ElasticLowLevelClient(settings);


            var searchResponse = lowlevelClient.Search<StringResponse>("products","product", @"
{   
    ""query"": {
    ""multi_match"" : {
                ""query"":      ""TOTACHI 5w  масло"",
      ""type"":       ""cross_fields"",
      ""fields"":     [ ""name"" ],
      ""operator"":   ""and""
    }
}
}
");


            var searchResponse2 = lowlevelClient.Search<StringResponse>("products", "product", PostData.Serializable(new
            {
                query = new
                {
                    multi_match = new
                    {
                        fields = new []{ "name"},
                        query = "TOTACHI 5w  масло",
                        type= "cross_fields",
                        @operator = "and"
                    }
                }
            }));

            

            var successful = searchResponse2.Success;
            var responseJson = searchResponse2.Body;

          var results=  JObject.Parse(responseJson);

            StringBuilder sb = new StringBuilder();
            foreach (var result in results["hits"]["hits"])
            {

               var prod= result["_source"].ToObject<ProductDto>();
                sb.AppendLine(prod.Name);

            }

            return Content(sb.ToString());
        }
    }
}