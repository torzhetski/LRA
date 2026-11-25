namespace LRA.Shared.Utils.Exceptions;

public class InvalidVerificationException : Exception
{
    public InvalidVerificationException()
        : base($"Not verified.") { }
}
