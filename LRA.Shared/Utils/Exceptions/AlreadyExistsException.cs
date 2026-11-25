using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRA.Shared.Utils.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException (string Name)
        : base($"Entity '{Name}' already exists.") { }
}
