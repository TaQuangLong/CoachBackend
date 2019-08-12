using System;
using System.Collections.Generic;
using System.Text;

namespace Dgm.Core.SeedWork
{
    public abstract class Enumeration: IComparable
    {
        public string Name { get; set; }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
