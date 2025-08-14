namespace EmailPush.Application.Utils;

/// <summary>
/// Email validation utility class
/// Handles all email-related validation logic
/// </summary>
public static class EmailValidator
{
    /// <summary>
    /// Validates if the provided email address has a valid format
    /// </summary>
    /// <param name="email">Email address to validate</param>
    /// <returns>True if email format is valid, false otherwise</returns>
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
    /// Validates a list of email addresses
    /// </summary>
    /// <param name="emails">List of email addresses to validate</param>
    /// <returns>List of invalid email addresses</returns>
    public static List<string> GetInvalidEmails(IEnumerable<string> emails)
    {
        return emails.Where(email => !IsValidEmail(email)).ToList();
    }

    /// <summary>
    /// Checks if all emails in the list are valid
    /// </summary>
    /// <param name="emails">List of email addresses to validate</param>
    /// <returns>True if all emails are valid, false otherwise</returns>
    public static bool AreAllEmailsValid(IEnumerable<string> emails)
    {
        return emails.All(IsValidEmail);
    }
}