using System;

namespace WebAppCore.Models
{
    public class KendoModel
    {
        public DateTime StartDate { get; set; }
        public string[] Cities { get; set; } =new [] { "1", "MSK", "NY" };
        public string City { get; set; }
    }
}