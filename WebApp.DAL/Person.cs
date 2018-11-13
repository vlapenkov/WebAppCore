using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.DAL
{
    public class Person
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        //[DefaultValue(100)]
        public int Age { get; set; }


        public bool IsMarried { get; set; }

        public bool IsAutoCreated { get; set; }

        public bool TestProp1 { get; set; }

        //    public int Salary { get; set; }

        public override string ToString()
        {

            return $"Person name is {this.Name}, and Age  is {this.Age} with hashCode {this.GetHashCode()}";
        }

    }

   


    
}
