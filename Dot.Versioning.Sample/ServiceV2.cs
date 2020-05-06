using System;
using System.Collections.Generic;
using System.Text;

namespace Dot.Versioning.Sample
{
    [Versioning(Version = "2.0")]
    public class ServiceV2 : IService
    {
        public void Display()
        {
            Console.WriteLine("IService v2");
        }
    }
}
