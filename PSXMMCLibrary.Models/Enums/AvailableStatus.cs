using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSXMMCLibrary.Models.Enums
{
    /// <summary>
    /// Possible states for a save block's availability
    /// </summary>
    public enum AvailableStatus
    {
        OpenBlock,
        FirstLink,
        MiddleLink,
        LastLink,
        Unusable
    }
}
