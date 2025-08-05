namespace EmailPush.Domain.Messages;

public class EmailCampaignMessage
{
    public Guid CampaignId { get; set; }
    public List<string> Recipients { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}