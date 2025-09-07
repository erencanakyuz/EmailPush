using EmailPush.Domain.ValueObjects;
using EmailPush.Domain.Models;

namespace EmailPush.Domain.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(Email recipient, string subject, string content, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmailSendResult>> SendBulkEmailAsync(RecipientList recipients, string subject, string content, CancellationToken cancellationToken = default);
}