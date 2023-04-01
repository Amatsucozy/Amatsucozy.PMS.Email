namespace Amatsucozy.PMS.Email.Core.Templates.Exceptions;

public sealed class PlaceholderNotClosedException : Exception
{
    public PlaceholderNotClosedException(string placeholderKey)
        : base($"Email body contains a placeholder that is not closed: {placeholderKey}.")
    {
    }
}