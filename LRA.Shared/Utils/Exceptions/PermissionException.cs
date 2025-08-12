namespace LRA.Shared.Utils.Exceptions;

public class PermissionException : Exception
{
    public PermissionException (string Name)
        : base($"User '{Name}' dont have permission to execute this action.") { }
}
