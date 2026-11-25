namespace LRA.Shared.Utils.Exceptions;

public class UnknownEventTypeException : Exception
{
    public UnknownEventTypeException(object eventType) : base($"Event '{eventType}' is unknown") { }
}
