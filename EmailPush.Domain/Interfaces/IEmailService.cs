using EmailPush.Domain.ValueObjects;

namespace EmailPush.Domain.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(Email recipient, string subject, string content, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmailSendResult>> SendBulkEmailAsync(RecipientList recipients, string subject, string content, CancellationToken cancellationToken = default);
}

public class EmailSendResult
{
    public string RecipientEmail { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime SentAt { get; set; }
}