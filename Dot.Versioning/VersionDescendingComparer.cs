using System.Collections.Generic;

namespace Dot.Versioning
{
    internal class VersionDescendingComparer : IComparer<decimal>
    {
        public int Compare(decimal x, decimal y)
        {
            if (x == y) return 0;
            if (x > y) return -1;
            return 1;
        }
    }
}