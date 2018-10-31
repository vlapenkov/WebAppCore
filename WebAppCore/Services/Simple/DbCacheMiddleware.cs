using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Data;
using WebAppCore.Models;

namespace WebAppCore.Services
{
   
        public class DbCacheMiddleware
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IMemoryCache _memoryCache;
            private readonly RequestDelegate _next;

            public DbCacheMiddleware(ApplicationDbContext dbContext, IMemoryCache memoryCache, RequestDelegate next)
            {
                _dbContext = dbContext;
                _memoryCache = memoryCache;
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                string cacheKey = "vipperson";
                Person person;

                // пытаемся получить элемент из кэша
                if (!_memoryCache.TryGetValue(cacheKey, out person))
                {
                    // если в кэше не найден элемент, получаем его от сервиса
                    person = await _dbContext.Persons.FirstAsync();
                    // и сохраняем в кэше
                    _memoryCache.Set(cacheKey, person,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
                }

                if (person == null)
                    await context.Response.WriteAsync("<p>Best Phone: Not available</p>");
                else
                    await context.Response.WriteAsync($"<p>Best Phone: {person.Name}</p>");
                await _next.Invoke(context);
            }
        }
        public static class DbCacheExtensions
        {
            public static IApplicationBuilder UseFirstPersonCache(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<DbCacheMiddleware>();
            }
        }
    
}
