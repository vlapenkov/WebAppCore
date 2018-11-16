using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApp.DAL
{ 
    /// <summary>
    /// Класс для логирования исключений
    /// </summary>
  public  class ErrorLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(255)]
        public string Username { get; set; }

        [MaxLength(255)]
        public string Path { get; set; }
                
        public string Exception { get; set; }

        public DateTime DateCreated { get; set; }

    }
}
