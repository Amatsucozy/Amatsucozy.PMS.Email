using System.Text;
using Amatsucozy.PMS.Email.Core.Templates.Exceptions;
using Amatsucozy.PMS.Shared.Core.Modelling;
using Amatsucozy.PMS.Shared.Core.Results;

namespace Amatsucozy.PMS.Email.Core.Templates;

public sealed class EmailTemplate : Entity
{
    public required string Key { get; init; }

    public required string Subject { get; init; }

    public required string PlainBody { get; init; }

    public required string Body { get; init; }

    public bool EnableMultipleReceivers { get; init; }

    public Result<string> BuildHtmlTemplate(IDictionary<string, string> placeholderValues)
    {
        if (string.IsNullOrWhiteSpace(Body))
        {
            return new EmailBodyIsBlankException();
        }

        var startIndex = Body.IndexOf("{{", 0, StringComparison.Ordinal);
        var endIndex = Body.IndexOf("}}", startIndex, StringComparison.Ordinal);

        if (startIndex == -1 || endIndex == -1)
        {
            return Body;
        }

        var sb = new StringBuilder();
        sb.Append(Body[..startIndex]);

        do
        {
            var placeholderKey = Body.Substring(startIndex + 2, endIndex - startIndex - 2);

            if (!placeholderValues.ContainsKey(placeholderKey))
            {
                return new PlaceholderWithoutValueException(placeholderKey);
            }

            sb.Append(placeholderValues[placeholderKey]);
            startIndex = Body.IndexOf("{{", endIndex, StringComparison.Ordinal);

            if (startIndex == -1)
            {
                sb.Append(Body[(endIndex + 2)..]);
                break;
            }

            sb.Append(Body[(endIndex + 2)..startIndex]);
            endIndex = Body.IndexOf("}}", startIndex, StringComparison.Ordinal);

            if (endIndex == -1)
            {
                return new PlaceholderNotClosedException(placeholderKey);
            }
        } while (true);

        return sb.ToString();
    }
}