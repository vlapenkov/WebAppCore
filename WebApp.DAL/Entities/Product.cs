using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.DAL
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [ForeignKey("Producer")]
        public int ProducerId { get; set; }

        [Required]
        [MaxLength(Byte.MaxValue)]
        public string Name { get; set; }

     [MaxLength(Byte.MaxValue)]
        public string Description { get; set; } 

        public byte Rating { get; set; }
        public Producer Producer { get; set; }
    }
}
