using EmailPush.Domain.Interfaces;
using EmailPush.Domain.ValueObjects;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EmailPush.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(Email recipient, string subject, string content, CancellationToken cancellationToken = default)
    {
        try
        {
            var message = CreateEmailMessage(recipient.Value, subject, content);
            await SendMessageAsync(message, cancellationToken);
            
            _logger.LogInformation("Email sent successfully to {Recipient}", recipient.Value);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient}", recipient.Value);
            return false;
        }
    }

    public async Task<IEnumerable<EmailSendResult>> SendBulkEmailAsync(RecipientList recipients, string subject, string content, CancellationToken cancellationToken = default)
    {
        var results = new List<EmailSendResult>();
        var semaphore = new SemaphoreSlim(10); // Limit concurrent sends to 10

        var tasks = recipients.Recipients.Select(async recipient =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                var success = await SendEmailAsync(recipient, subject, content, cancellationToken);
                return new EmailSendResult
                {
                    RecipientEmail = recipient.Value,
                    IsSuccess = success,
                    SentAt = DateTime.UtcNow,
                    ErrorMessage = success ? null : "Failed to send email"
                };
            }
            catch (Exception ex)
            {
                return new EmailSendResult
                {
                    RecipientEmail = recipient.Value,
                    IsSuccess = false,
                    SentAt = DateTime.UtcNow,
                    ErrorMessage = ex.Message
                };
            }
            finally
            {
                semaphore.Release();
            }
        });

        results = (await Task.WhenAll(tasks)).ToList();

        _logger.LogInformation(
            "Bulk email send completed. Sent: {SuccessCount}/{TotalCount}",
            results.Count(r => r.IsSuccess),
            results.Count);

        return results;
    }

    private MimeMessage CreateEmailMessage(string recipient, string subject, string content)
    {
        var message = new MimeMessage();
        
        var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "noreply@emailpush.com";
        var fromName = _configuration["EmailSettings:FromName"] ?? "EmailPush";
        
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(new MailboxAddress("", recipient));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = content,
            TextBody = System.Text.RegularExpressions.Regex.Replace(content, "<[^>]*>", "") // Strip HTML for text version
        };

        message.Body = bodyBuilder.ToMessageBody();
        
        return message;
    }

    private async Task SendMessageAsync(MimeMessage message, CancellationToken cancellationToken)
    {
        using var client = new SmtpClient();

        // Get SMTP configuration
        var smtpHost = _configuration["EmailSettings:SmtpHost"] ?? "localhost";
        var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
        var username = _configuration["EmailSettings:Username"];
        var password = _configuration["EmailSettings:Password"];
        var useSSL = bool.Parse(_configuration["EmailSettings:UseSSL"] ?? "true");

        // Connect to SMTP server
        await client.ConnectAsync(smtpHost, smtpPort, 
            useSSL ? SecureSocketOptions.StartTls : SecureSocketOptions.None, 
            cancellationToken);

        // Authenticate if credentials are provided
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            await client.AuthenticateAsync(username, password, cancellationToken);
        }

        // Send message
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}