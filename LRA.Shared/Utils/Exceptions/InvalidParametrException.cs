namespace LRA.Shared.Utils.Exceptions;

public class InvalidParametrException : Exception
{
    public InvalidParametrException()
        : base($"Entered invalid parametrs.") { }
}
