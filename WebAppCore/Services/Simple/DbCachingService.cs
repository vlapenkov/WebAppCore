using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAppCore.Data;
using WebAppCore.Models;

namespace WebAppCore.Services
{
    /// <summary>
    /// Возвращает из memcache  первого Person , memcache обнуляется раз в 30 секунд TimeSpan.FromSeconds(10) (для примера)
    /// </summary>
    public class   DbSetCachingService<T>  where T:class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private static readonly int timeInSeconds = 10;//60*60*24;
        private static readonly object _sync =new object();

        public DbSetCachingService(ApplicationDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;            
        }

        private static int numberOfRecords = 0;
        /// <summary>
        /// Get all table from cache
        /// </summary>
        public IEnumerable<T> All()
        {

            int numOfRecordsInDb= _dbContext.Set<T>().Count();
            
                string cacheKey = $"{typeof(T).Name}.all";
            lock (_sync)
            {
                if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<T> persons) && numOfRecordsInDb == numberOfRecords) return persons;
                else
                {
                    numberOfRecords = numOfRecordsInDb;
                    persons = _dbContext.Set<T>().AsEnumerable();
                    _memoryCache.Set(cacheKey, persons, new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(timeInSeconds)));
                    return persons;
                }
            }
            
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
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(timeInSeconds)));
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
