namespace EmailPush.Domain.Messages;

public class EmailCampaignMessage
{
    public Guid CampaignId { get; set; }
    public string CampaignName { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Recipients { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}