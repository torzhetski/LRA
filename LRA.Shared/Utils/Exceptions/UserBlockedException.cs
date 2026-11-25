using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRA.Shared.Utils.Exceptions;

public class UserBlockedException : Exception
{
    public UserBlockedException(string Name)
        : base($"Account '{Name}' is blocked.") { }
}
