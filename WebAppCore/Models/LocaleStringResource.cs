using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Models
{
    public partial class LocaleStringResource : BaseEntity
    {
        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        
        [ForeignKey("Language")]
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the resource name
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the resource value
        /// </summary>
        public string ResourceValue { get; set; }

        /// <summary>
        /// Gets or sets the language
        /// </summary>
        public virtual Language Language { get; set; }
    }
}
