namespace Amatsucozy.PMS.Email.Core.Templates.Exceptions;

public sealed class EmailBodyIsBlankException : Exception
{
    public EmailBodyIsBlankException()
        : base("Email body cannot be null or empty.")
    {
    }
}