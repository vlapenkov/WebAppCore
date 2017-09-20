using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Services
{
   
        public class HelloMiddleware
        {
            private readonly RequestDelegate _next;

            public HelloMiddleware(RequestDelegate next)
            {
                this._next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                context.Session.SetString("mw1", "test");
                //  await context.Response.WriteAsync("<p>Begin Hello World1</p>");
                context.Items["first"] = DateTime.Now;
                await _next.Invoke(context);
                //   await context.Response.WriteAsync("<p>End Hello World1</p>");
            }
        }
        public static class HelloExtensions
        {
            public static IApplicationBuilder UseHello(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<HelloMiddleware>();
            }
        }
    
}
