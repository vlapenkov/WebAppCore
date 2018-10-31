using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Services
{

    public interface ISimple {
        string ToString();
    }

    public class ClassA : ISimple
    {
        public override string ToString() => "A";

    }

    public class ClassB : ISimple
    {
        public override string ToString() => "B";
    }

   
}
