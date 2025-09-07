namespace EmailPush.Domain.Models;

public class EmailSendResult
{
    public string RecipientEmail { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime SentAt { get; set; }
}