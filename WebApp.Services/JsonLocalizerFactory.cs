using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Services
{
   public class JsonLocalizerFactory : IStringLocalizerFactory
    {
        public IStringLocalizer Create(Type resourceSource)
        {
            return CreateStringLocalizer();
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return CreateStringLocalizer();

        }

        private IStringLocalizer CreateStringLocalizer()
        {
            return ServiceLocator.Current.GetInstance<IStringLocalizer>();
            
        }

           
    }
}
