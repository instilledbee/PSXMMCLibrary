using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSXMMCLibrary
{
    /// <summary>
    /// Overrides the .NET Contracts to forego rewriting the assembly with their usage.
    /// </summary>
    public static class Contract
    {
        public static void Requires<TException>(bool Predicate)
           where TException : Exception, new()
        {
            if (!Predicate)
            {
                throw new TException();
            }
        }

        public static void Requires(bool Predicate)
        {
            Requires<Exception>(Predicate);
        }
    }
}
