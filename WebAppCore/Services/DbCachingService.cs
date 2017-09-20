using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Data;
using WebAppCore.Models;

namespace WebAppCore.Services
{
    /// <summary>
    /// Возвращает из memcache  первого Person , memcache обнуляется раз в 10 секунд (для примера)
    /// </summary>
    public class DbCachingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public DbCachingService(ApplicationDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public Person FirstPerson
        {

            get
            {

                string cacheKey = "vipperson";
                Person person;

                // пытаемся получить элемент из кэша
                if (!_memoryCache.TryGetValue(cacheKey, out person))
                {
                    // если в кэше не найден элемент, получаем его от сервиса
                    person = _dbContext.Persons.FirstOrDefault();
                    // и сохраняем в кэше
                    _memoryCache.Set(cacheKey, person,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
                }
                Person personFromCache;

                // пытаемся получить элемент из кэша
                if (_memoryCache.TryGetValue(cacheKey, out personFromCache))
                    return personFromCache;

                return null;
            }
        }
    }
}
