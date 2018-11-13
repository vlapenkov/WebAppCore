using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.DAL
{
    
        /// <summary>
        /// Represents a language
        /// </summary>
        public partial class Language :BaseEntity
        {
          

        /// <summary>
        /// Gets or sets the language culture
        /// </summary>
          [MaxLength(25)]
        public string LanguageCulture { get; set; }
        }
}
