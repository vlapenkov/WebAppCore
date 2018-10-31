using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Services
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString MyOwnHtmlHelper(this HtmlHelper helper, string message)
        {
            return new HtmlString($"<span>{message}<span>");
        }
    }
}
