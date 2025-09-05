using System.Text.RegularExpressions;

namespace EmailPush.Application.Utils;

public static class EmailValidator
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled);

    public static bool IsValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return EmailRegex.IsMatch(email);
    }

    public static bool ValidateList(IEnumerable<string> emails)
    {
        return emails.All(IsValid);
    }

    public static List<string> GetInvalidEmails(IEnumerable<string> emails)
    {
        return emails.Where(email => !IsValid(email)).ToList();
    }
}