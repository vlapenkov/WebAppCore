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
using WebApp.Services.Extensions;

namespace WebApp.Services
{
    /// <summary>
    /// Сервис для локализации 
    /// </summary>
    public class JsonLocalizationService : IStringLocalizer
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


        public virtual string GetResource(string name) => GetStringSafely(name)??name;
        

        public virtual string GetLocalizedEnum<TEnum>(TEnum enumValue, int? languageId = null) where TEnum : struct
        {
            string result = string.Empty;
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            //localized value
            var resourceName = $"{NopLocalizationDefaults.EnumLocaleStringResourcesPrefix}{typeof(TEnum)}.{enumValue}";
             result = GetResource(resourceName);// GetResource(resourceName, languageId ?? defaultWorkingLanguageId, false, string.Empty, true);


            //set default value if required
            if ( string.IsNullOrEmpty(result))
                result = CommonHelper.ConvertEnum(enumValue.ToString());
            

            return result;
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

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {

            var culture = CultureInfo.CurrentUICulture;
            var resources = _resourcesCache.GetOrAdd(culture.Name, _ => GetAllStringsByCulture(culture)

            );

            return resources.Select(r => new LocalizedString(r.Key, r.Value));
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;
            return new JsonLocalizationService(_hostingEnvironment,_resourcesPath);
        }
        #endregion
    }
}
