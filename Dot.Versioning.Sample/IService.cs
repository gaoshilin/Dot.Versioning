using System;
using System.Collections.Generic;
using System.Text;

namespace Dot.Versioning.Sample
{
    public interface IService : IVersioningService
    {
        void Display();
    }
}