using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Services
{
    /// <summary>
    /// Сервис для локализации 
    /// </summary>
    public class JsonLocalizationService
    {
        private readonly ConcurrentDictionary<string, IEnumerable<KeyValuePair<string, string>>> _resourcesCache = new ConcurrentDictionary<string, IEnumerable<KeyValuePair<string, string>>>();
        private readonly string _resourcesPath = "Resources";       

        private readonly IHostingEnvironment _hostingEnvironment;    

        public JsonLocalizationService(          
            IHostingEnvironment hostingEnvironment,
            string resourcesPath
           )
        {
            _hostingEnvironment = hostingEnvironment;
            _resourcesPath = resourcesPath;
        }

        public LocalizedString this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }

                var value = GetStringSafely(name);

                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }
        
        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }

                var format = GetStringSafely(name);
                var value = string.Format(format ?? name, arguments);

                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        #region Utils

        private string[] GetFilesByCulture(CultureInfo culture)
        {
            var rootPath = Path.Combine(_hostingEnvironment.ContentRootPath, _resourcesPath);
            return Directory.GetFiles(rootPath, $"*.{culture}.json");
        }

        protected IEnumerable<KeyValuePair<string,string>> GetAllStringsByCulture(CultureInfo currentCulture)
        {

            IEnumerable<KeyValuePair<string, string>> value = null;
            var files = GetFilesByCulture(currentCulture);


            if (files.Length > 0)
            {
                var builder = new ConfigurationBuilder().SetBasePath(Path.Combine(_hostingEnvironment.ContentRootPath, _resourcesPath));

                foreach (var file in files)
                {
                    builder.AddJsonFile(file, optional: false, reloadOnChange: true);
                }

                var config = builder.Build();
                value = config.AsEnumerable();
            }

            return value;
        }

        

        protected string GetStringSafely(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var culture = CultureInfo.CurrentUICulture;
            var resources = _resourcesCache.GetOrAdd(culture.Name, _ => GetAllStringsByCulture(culture)

            );
            var resource = resources?.SingleOrDefault(s => s.Key == name);
        
            return resource?.Value ?? null;
        }
        #endregion
    }
}
