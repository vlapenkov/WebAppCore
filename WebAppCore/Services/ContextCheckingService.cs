using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Services
{
    public class ContextCheckingService
    {
        IHttpContextAccessor _httpContextAccessor;

        public ContextCheckingService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public object GetField(string fieldname)
        {
           return _httpContextAccessor.HttpContext.Items[fieldname];
        }

    }
}
