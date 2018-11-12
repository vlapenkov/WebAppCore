using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Models
{
    /// <summary>
    /// Используется исключительно для для elasticsearch
    /// </summary>
    public class ProductDto
    {
        public string Name { get; set; }
        public string Article { get; set; }
    }
}
