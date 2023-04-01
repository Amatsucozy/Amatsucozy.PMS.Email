namespace Amatsucozy.PMS.Email.Core.Templates.Exceptions;

public sealed class PlaceholderWithoutValueException : Exception
{
    public PlaceholderWithoutValueException(string placeholderKey)
        : base($"Email body contains a placeholder with no value: {placeholderKey}.")
    {
    }
}