namespace EmailPush.Application.Utils;

/// <summary>
/// Checks if emails are valid
/// </summary>
public static class EmailValidator
{
    /// <summary>
    /// Checks if email is valid
    /// </summary>
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets invalid emails from list
    /// </summary>
    public static List<string> GetInvalidEmails(IEnumerable<string> emails)
    {
        return emails.Where(email => !IsValidEmail(email)).ToList();
    }

    /// <summary>
    /// Checks if all emails are valid
    /// </summary>
    public static bool AreAllEmailsValid(IEnumerable<string> emails)
    {
        return emails.All(IsValidEmail);
    }
}