using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.DAL
{
    public class Producer
    {
        public Producer()
        {
            Products = new List<Product>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(Byte.MaxValue)]
        public string Name { get; set; }


        public ICollection<Product> Products { get; set; }

        public bool IsActive { get; set; }

    }
}