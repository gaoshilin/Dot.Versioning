using System;
using System.Collections.Generic;
using System.Text;

namespace Dot.Versioning.Sample
{
    [Versioning(Version = "1.0")]
    public class ServiceV1 : IService
    {
        public void Display()
        {
            Console.WriteLine("IService v1");
        }
    }
}
