namespace EmailPush.Domain.Entities;

public class Campaign
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Recipients { get; set; } = new();
    public CampaignStatus Status { get; set; } = CampaignStatus.Draft;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public int TotalRecipients => Recipients.Count;
    public int SentCount { get; set; } = 0;

    public bool CanBeDeleted => Status == CampaignStatus.Draft;
    public bool CanBeUpdated => Status == CampaignStatus.Draft;
    public bool CanBeStarted => Status == CampaignStatus.Draft;

    public void StartSending()
    {
        if (Status != CampaignStatus.Draft)
            throw new InvalidOperationException("Campaign can only be started from Draft status");
            
        Status = CampaignStatus.Sending;
        StartedAt = DateTime.UtcNow;
    }

    public void MarkEmailAsSent()
    {
        if (Status != CampaignStatus.Sending)
            throw new InvalidOperationException("Cannot mark email as sent - campaign is not in sending status");
            
        SentCount++;
        
        if (SentCount >= TotalRecipients)
        {
            Status = CampaignStatus.Completed;
        }
    }
}

public enum CampaignStatus
{
    Draft,
    Ready,
    Sending,
    Completed,
    Failed
}